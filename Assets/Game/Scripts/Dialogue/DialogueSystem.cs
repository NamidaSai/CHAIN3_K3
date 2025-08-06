using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Dialogue
{
    public class DialogueSystem : MonoBehaviour
    {
        public static DialogueSystem Instance;

        [SerializeField] private Choice continueChoice;
        [SerializeField] private Choice closeChoice;
        
        private DialoguePart _currentDialogue;
        private int _currentLineIndex = 0;

        [HideInInspector]
        public UnityEvent<DialogueLine> onPlayLine;
        [HideInInspector] 
        public UnityEvent<List<Choice>> onDisplayChoices;
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

        public void HandleChoiceMade(Choice choice)
        {
            if (!string.IsNullOrEmpty(choice.flagToCreate))
            {
                CHAIN_SharedData.CreateFlag(choice.flagToCreate);
            }
            
            if (choice.text == continueChoice.text)
            {
                ProcessDialogue();
                return;
            }
            
            if (choice.nextPart)
            {
                StartDialogue(choice.nextPart);
                return;
            }
            
            EndDialogue();
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
                PlayNextDialogueLine();
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
            _currentDialogue = null;
        }

        private void PlayNextDialogueLine()
        {
            DialogueLine lineToPlay = _currentDialogue.dialogueLines[_currentLineIndex];
            
#if UNITY_EDITOR
            Debug.Log($"{nameof(DialogueSystem)}.{nameof(PlayNextDialogueLine)}: {lineToPlay.text}.");
#endif           
            
            onPlayLine?.Invoke(lineToPlay);
            _currentLineIndex++;

            DisplayChoices();
        }

        private void DisplayChoices()
        {
            bool isLastLine = _currentLineIndex == _currentDialogue.dialogueLines.Count;
            if (!isLastLine)
            {
                onDisplayChoices?.Invoke(new List<Choice> { continueChoice });
                return;
            }
            
            bool dialogueHasChoices = _currentDialogue.choices.Count > 0;
            onDisplayChoices?.Invoke
            (
                dialogueHasChoices 
                    ? _currentDialogue.choices 
                    : new List<Choice> {closeChoice}
            );
        }
    }
}