using System;
using UnityEngine;

namespace Game.Dialogue
{
    [RequireComponent(typeof(DialogueSelector))]
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private bool isRepeatable = false;

        private DialogueSelector _dialogueSelector;
        private bool _wasTriggered = false;

        private void Awake()
        {
            _dialogueSelector = GetComponent<DialogueSelector>();
        }

        public void Trigger()
        {
            if (_wasTriggered)
            {
                return;
            }
            
            _wasTriggered = true;
            DialoguePart dialogueToPlay = _dialogueSelector.SelectDialogueByFlag();
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