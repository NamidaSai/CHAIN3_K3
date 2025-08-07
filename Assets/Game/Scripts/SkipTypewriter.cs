using Febucci.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    [RequireComponent(typeof(TypewriterByCharacter))]
    public class SkipTypewriter : MonoBehaviour
    {
        private InputAction _interactAction;
        private TypewriterByCharacter _typewriter;
        
        private void Awake()
        {
            _interactAction = InputSystem.actions.FindAction("Interact");
            _interactAction.performed += HandleInteract;
            _typewriter = GetComponent<TypewriterByCharacter>();
        }

        private void OnDestroy()
        {
            _interactAction.performed -= HandleInteract;
        }

        private void HandleInteract(InputAction.CallbackContext context)
        {
            _typewriter.SkipTypewriter();
        }
    }
}
