using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private DialoguePart dialogueToPlay;

        private bool _wasTriggered = false;

        public void Trigger()
        {
            if (_wasTriggered)
            {
                return;
            }
            
            _wasTriggered = true;
            DialogueSystem.Instance.StartDialogue(dialogueToPlay);
        }
    }
}