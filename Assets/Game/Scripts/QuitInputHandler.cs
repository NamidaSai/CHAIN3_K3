using Game.Interact;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class QuitInputHandler : MonoBehaviour
    {
        [SerializeField] private Canvas quitCanvas;
        
        private InputAction _quitAction;
        
        private void Awake()
        {
            _quitAction = InputSystem.actions.FindAction("Quit");
            _quitAction.performed += HandleQuit;
            quitCanvas.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _quitAction.performed -= HandleQuit;
        }

        private void HandleQuit(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR
            Debug.Log("Quit");
#endif
            quitCanvas.gameObject.SetActive(!quitCanvas.gameObject.activeSelf);
            InteractSystem.Instance.IsBlocked = !quitCanvas.gameObject.activeSelf;
        }
        
        public void QuitGame()
        {
            Application.Quit();
        }
    }
}