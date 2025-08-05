using Unity.Cinemachine;
using UnityEngine;

namespace Game
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera interactCamera;
        [SerializeField] private AnimationHandler animationHandler;

        private bool _isHovered = false;
        private bool _isInteracted = false;

        private void Awake()
        {
            if (interactCamera != null)
                interactCamera.gameObject.SetActive(false);
        }

        public void Interact(InteractSystem interactor)
        {
            if (_isInteracted) return;

            interactor.onInteract.AddListener(HandleInteract);
            interactor.onReturn.AddListener(HandleReturn);

            if (interactCamera != null)
                interactCamera.gameObject.SetActive(true);

            animationHandler?.SetTrigger("interact");
            _isInteracted = true;
        }

        private void HandleInteract(InteractSystem interactor)
        {
            interactor.onInteract.RemoveListener(HandleInteract);

            if (interactCamera != null)
                interactCamera.gameObject.SetActive(false);

            animationHandler?.SetTrigger("default");
            _isInteracted = false;
        }

        private void HandleReturn(InteractSystem interactor)
        {
            interactor.onReturn.RemoveListener(HandleReturn);

            if (interactCamera != null)
                interactCamera.gameObject.SetActive(false);

            animationHandler?.SetTrigger("default");
            _isInteracted = false;
        }

        public void OnHoverEnter(InteractSystem interactor)
        {
            if (_isHovered) return;

            animationHandler?.SetTrigger("hoverStart");
            _isHovered = true;
        }

        public void OnHoverExit(InteractSystem interactor)
        {
            if (!_isHovered) return;

            animationHandler?.SetTrigger("hoverEnd");
            _isHovered = false;
        }
    }
}

