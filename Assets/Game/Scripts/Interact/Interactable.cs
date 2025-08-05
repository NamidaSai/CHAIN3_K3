using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interact
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera interactCamera;
        [SerializeField] private AnimationHandler animationHandler;

        private bool _isHovered = false;
        private bool _isInteracted = false;

        public UnityEvent onInteract;

        private void Awake()
        {
            if (interactCamera == null) { return; }
            interactCamera.gameObject.SetActive(false);
            animationHandler.onInteractEnd.AddListener(HandleInteractEnd);
        }

        private void OnDestroy()
        {
            animationHandler.onInteractEnd.RemoveListener(HandleInteractEnd);
        }

        private void HandleInteractEnd()
        {
            onInteract?.Invoke();
        }

        public void Interact(InteractSystem interactor)
        {
            if (_isInteracted) { return; }

            interactor.onInteract.AddListener(HandleInteract);
            interactor.onReturn.AddListener(HandleReturn);

            if (interactCamera != null)
            {
                interactCamera.gameObject.SetActive(true);
            }

            animationHandler?.SetTrigger("interact");
            _isInteracted = true;
        }

        public void OnHoverEnter(InteractSystem interactor)
        {
            if (_isHovered) { return; }

            animationHandler?.SetTrigger("hoverStart");
            _isHovered = true;
        }

        public void OnHoverExit(InteractSystem interactor)
        {
            if (!_isHovered) { return; }

            animationHandler?.SetTrigger("hoverEnd");
            _isHovered = false;
        }
        
        private void HandleInteract(InteractSystem interactor)
        {
            interactor.onInteract.RemoveListener(HandleInteract);

            if (interactCamera != null)
            {
                interactCamera.gameObject.SetActive(false);
            }

            animationHandler?.SetTrigger("default");
            _isInteracted = false;
        }

        private void HandleReturn(InteractSystem interactor)
        {
            interactor.onReturn.RemoveListener(HandleReturn);

            if (interactCamera != null)
            {
                interactCamera.gameObject.SetActive(false);
            }

            animationHandler?.SetTrigger("default");
            _isInteracted = false;
        }
    }
}

