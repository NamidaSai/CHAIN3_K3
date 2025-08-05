using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Interact
{
    public class InteractSystem : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera primaryCamera;
        [SerializeField] private float maxInteractDistance = 5f;

        private Camera _mainCamera;
        private InputAction _interactAction;
        private InputAction _returnAction;

        [HideInInspector] public UnityEvent<InteractSystem> onInteract;
        [HideInInspector] public UnityEvent<InteractSystem> onReturn;
        [HideInInspector] public UnityEvent<Interactable> onHoverEnter;
        [HideInInspector] public UnityEvent<Interactable> onHoverExit;

        private Interactable _currentHovered;

        private void Awake()
        {
            _mainCamera = Camera.main;

            _interactAction = InputSystem.actions.FindAction("Interact");
            _returnAction = InputSystem.actions.FindAction("Return");

            _interactAction.performed += HandleInteract;
            _returnAction.performed += HandleReturn;
        }

        private void OnDestroy()
        {
            _interactAction.performed -= HandleInteract;
            _returnAction.performed -= HandleReturn;
        }

        private void Update()
        {
            Interactable newHover = GetInteractableUnderCursor();

            if (newHover == _currentHovered) { return; }
            
            if (_currentHovered)
            {
                onHoverExit?.Invoke(_currentHovered);
                _currentHovered.OnHoverExit(this);
            }

            if (newHover)
            {
                onHoverEnter?.Invoke(newHover);
                newHover.OnHoverEnter(this);
            }

            _currentHovered = newHover;
        }

        private void HandleInteract(InputAction.CallbackContext context)
        {
            if (_currentHovered == null) { return; }
            Interact(_currentHovered);
        }

        private void HandleReturn(InputAction.CallbackContext context)
        {
            onReturn?.Invoke(this);
            primaryCamera.gameObject.SetActive(true);
        }

        private void Interact(Interactable target)
        {
            onInteract?.Invoke(this);
            target.Interact(this);
            primaryCamera.gameObject.SetActive(false);
        }

        private Interactable GetInteractableUnderCursor()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, maxInteractDistance)) { return null; }
            
            GameObject hitObject = hit.collider.gameObject;
            return hitObject.CompareTag("Interactable") 
                ? hitObject.GetComponent<Interactable>() 
                : null;
        }
    }
}