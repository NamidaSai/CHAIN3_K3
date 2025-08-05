using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class QuitInputHandler : MonoBehaviour
    {
        private InputAction _quitAction;
        
        private void Awake()
        {
            _quitAction = InputSystem.actions.FindAction("Quit");
            _quitAction.performed += HandleQuit;
        }

        private void OnDestroy()
        {
            _quitAction.performed -= HandleQuit;
        }

        private void HandleQuit(InputAction.CallbackContext context)
        {
            Debug.Log("Quit");
            QuitGame();
        }
        
        private void QuitGame()
        {
            Application.Quit();
        }
    }
}