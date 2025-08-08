using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interact
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private string hoverSoundID;
        [SerializeField] private CinemachineCamera interactCamera;
        [SerializeField] private AnimationHandler animationHandler;

        private bool _isHovered = false;
        private bool _isInteracted = false;

        public bool CanBeInteractedWith { private get; set; } = true;

        public UnityEvent onInteract;

        private void Awake()
        {
            if (interactCamera == null) { return; }
            interactCamera.gameObject.SetActive(false);
        }

        public void Interact(InteractSystem interactor)
        {
            if (!CanBeInteractedWith) { return; }
            
            if (_isInteracted) { return; }

            onInteract?.Invoke();
            
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
            if (!CanBeInteractedWith) { return; }
            if (_isHovered) { return; }

            animationHandler?.SetTrigger("hoverStart");
            _isHovered = true;
        }

        public void OnHoverExit(InteractSystem interactor)
        {
            if (!CanBeInteractedWith) { return; }
            if (!_isHovered) { return; }

            animationHandler?.SetTrigger("hoverEnd");
            _isHovered = false;
        }

        public void ActivateCamera(bool active)
        {
            interactCamera.gameObject.SetActive(active);
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

        public string GetHoverSoundID()
        {
            return hoverSoundID;
        }
    }
}

