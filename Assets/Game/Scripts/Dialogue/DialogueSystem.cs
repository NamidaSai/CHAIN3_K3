using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        public static DialogueSystem Instance;
        
        private DialoguePart _currentDialogue;
        private int _currentLineIndex = 0;

        [HideInInspector]
        public UnityEvent<DialogueLine> onPlayLine;
        [HideInInspector]
        public UnityEvent onDialogueStart;
        [HideInInspector]
        public UnityEvent onDialogueEnd;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            Instance = this;
        }

        public void StartDialogue(DialoguePart dialoguePart)
        {
#if UNITY_EDITOR
            Debug.Log($"{nameof(DialogueSystem)}.{nameof(StartDialogue)} called.");
#endif
            onDialogueStart?.Invoke();
            
            _currentDialogue = dialoguePart;
            _currentLineIndex = 0;
            
            ProcessDialogue();
        }

        public void ProcessDialogue()
        {
            if (!_currentDialogue)
            {
                Debug.LogWarning($"{nameof(DialogueSystem)}.{nameof(ProcessDialogue)}: No dialogue found.");
                return;
            }

            if (_currentLineIndex < _currentDialogue.dialogueLines.Count)
            {
                PlayNextDialogueLine(_currentDialogue);
                return;
            }

            EndDialogue();
        }

        private void EndDialogue()
        {
#if UNITY_EDITOR
            Debug.Log($"{nameof(DialogueSystem)}.{nameof(EndDialogue)} called.");
#endif            
            onDialogueEnd?.Invoke();
            
            if (_currentDialogue.choices.Count > 0)
            {
                StartDialogue(_currentDialogue.choices[0].nextPart);
                return;
            }
            
            _currentDialogue = null;
        }

        private void PlayNextDialogueLine(DialoguePart dialoguePart)
        {
            DialogueLine lineToPlay = dialoguePart.dialogueLines[_currentLineIndex];
            
#if UNITY_EDITOR
            Debug.Log($"{nameof(DialogueSystem)}.{nameof(PlayNextDialogueLine)}: {lineToPlay.text}.");
#endif           
            
            onPlayLine?.Invoke(lineToPlay);
            _currentLineIndex++;
        }
    }
}