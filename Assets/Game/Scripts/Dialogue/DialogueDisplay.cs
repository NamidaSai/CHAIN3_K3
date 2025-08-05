using TMPro;
using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueDisplay : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_Text characterDisplay;
        [SerializeField] private TMP_Text lineDisplay;

        // Dialogue System is initialised in Awake()
        private void Start()
        {
            DialogueSystem.Instance.onPlayLine.AddListener(HandlePlayLine);
            DialogueSystem.Instance.onDialogueStart.AddListener(HandleStartDialogue);
            DialogueSystem.Instance.onDialogueEnd.AddListener(HandleEndDialogue);
            
            canvas.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            DialogueSystem.Instance.onPlayLine.RemoveListener(HandlePlayLine);
            DialogueSystem.Instance.onDialogueStart.RemoveListener(HandleStartDialogue);
            DialogueSystem.Instance.onDialogueEnd.RemoveListener(HandleEndDialogue);
        }

        private void HandleStartDialogue()
        {
            canvas.gameObject.SetActive(true);
        }

        private void HandleEndDialogue()
        {
            canvas.gameObject.SetActive(false);
        }

        private void HandlePlayLine(DialogueLine lineToPlay)
        {
            string characterName = lineToPlay.character.ToString().ToUpper();
            characterDisplay.text = $"{characterName}:";
            lineDisplay.text = lineToPlay.text;
        }
    }
}