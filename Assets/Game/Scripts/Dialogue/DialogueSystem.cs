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

        public UnityEvent<DialogueLine> onPlayLine;
        public UnityEvent<List<Choice>> onDisplayChoices;
        public UnityEvent onDialogueStart;
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
            
            if (choice.nextPart)
            {
                StartDialogue(choice.nextPart);
                return;
            }
            
            if (choice.text == continueChoice.text)
            {
                ProcessDialogue();
                return;
            }
            
            EndDialogue();
        }

        public void StartDialogue(DialoguePart dialoguePart)
        {
#if UNITY_EDITOR
            Debug.Log($"{nameof(DialogueSystem)}.{nameof(StartDialogue)} called.");
#endif
            _currentDialogue = dialoguePart;
            _currentLineIndex = 0;
            
            onDialogueStart?.Invoke();
            
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

        public void EndDialogue()
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

            List<Choice> choicesToDisplay = new();
            foreach (Choice choice in _currentDialogue.choices)
            {
                if (!string.IsNullOrEmpty(choice.flagRequired)
                    && !CHAIN_SharedData.DoesFlagExist(choice.flagRequired))
                {
                    continue;
                }

                choicesToDisplay.Add(choice);
            }
            
            bool dialogueHasChoices = choicesToDisplay.Count > 0;
            onDisplayChoices?.Invoke
            (
                dialogueHasChoices 
                    ? choicesToDisplay
                    : new List<Choice> {closeChoice}
            );
        }

#if UNITY_EDITOR
        public string GetCurrentDialoguePart()
        {
            return _currentDialogue?.name;
        }
#endif
    }
}