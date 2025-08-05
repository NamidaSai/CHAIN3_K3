using Unity.Cinemachine;
using UnityEngine;

namespace Game
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private CinemachineCamera interactCamera;
        [SerializeField] private AnimationHandler animationHandler;

        private void Awake()
        {
            interactCamera.gameObject.SetActive(false);
        }

        public void Interact(InteractSystem interactor)
        {
            interactor.onInteract.AddListener(HandleInteract);
            interactor.onReturn.AddListener(HandleReturn);
            interactCamera.gameObject.SetActive(true);
            animationHandler.SetTrigger("interact");
        }

        private void HandleInteract(InteractSystem interactor)
        {
            interactor.onInteract.RemoveListener(HandleInteract);
            interactCamera.gameObject.SetActive(false);
            animationHandler.SetTrigger("default");
        }

        private void HandleReturn(InteractSystem interactor)
        {
            interactor.onReturn.RemoveListener(HandleReturn);
            interactCamera.gameObject.SetActive(false);
            animationHandler.SetTrigger("default");
        }
    }
}
