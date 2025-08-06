using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private DialoguePart dialogueToPlay;
        [SerializeField] private bool isRepeatable = false;

        private bool _wasTriggered = false;

        public void Trigger()
        {
            if (_wasTriggered)
            {
                return;
            }
            
            _wasTriggered = true;
            DialogueSystem.Instance.StartDialogue(dialogueToPlay);
            DialogueSystem.Instance.onDialogueEnd.AddListener(HandleDialogueEnd);
        }

        private void HandleDialogueEnd()
        {
            _wasTriggered = !isRepeatable;
            DialogueSystem.Instance.onDialogueEnd.RemoveListener(HandleDialogueEnd);
        }
    }
}