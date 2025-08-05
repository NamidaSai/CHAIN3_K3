using System;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game
{
    public class InteractSystem : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera primaryCamera;

        private Camera _mainCamera;
        private InputAction _interactAction;
        private InputAction _returnAction;
        
        [HideInInspector]
        public UnityEvent<InteractSystem> onInteract;
        [HideInInspector]
        public UnityEvent<InteractSystem> onReturn;

        [HideInInspector]
        public UnityEvent onHover;


        private void Awake()
        {
            _mainCamera = Camera.main;
            
            _interactAction = InputSystem.actions.FindAction("Interact");
            _returnAction = InputSystem.actions.FindAction("Return");

            _interactAction.performed += HandleInteract;
            _returnAction.performed += HandleReturn;
        }

        private void Update()
        {
            if (!IsHoveringInteractable(out _)) { return; }
            onHover?.Invoke();
        }

        private void HandleInteract(InputAction.CallbackContext context)
        {
            TryInteract();
        }

        private void TryInteract()
        {
            if (!IsHoveringInteractable(out GameObject target)) { return; }
            Interact(target);
        }

        private bool IsHoveringInteractable(out GameObject target)
        {
            target = null;
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity)) { return false; }

            target = hit.collider.gameObject;

            return target.CompareTag("Interactable");
        }

        private void Interact(GameObject target)
        {
            onInteract?.Invoke(this);
            
            Interactable interactable = target.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact(this); // or pass context
                primaryCamera.gameObject.SetActive(false);
            }
            else
            {
                Debug.LogWarning($"'{target.name}' is tagged Interactable but has no Interactable component.");
            }
        }

        private void HandleReturn(InputAction.CallbackContext context)
        {
            onReturn?.Invoke(this);
            primaryCamera.gameObject.SetActive(true);
        }
    }
}