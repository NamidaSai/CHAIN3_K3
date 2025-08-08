using System.Collections;
using UnityEngine;

namespace Game.Dialogue
{
    [RequireComponent(typeof(DialogueSelector))]
    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField] private float spawnDelay = 1f;

        private DialogueSelector _dialogueSelector;
        private bool _wasTriggered = false;

        private void Awake()
        {
            _dialogueSelector = GetComponent<DialogueSelector>();
        }

        /// <summary>
        /// Use to trigger from CHAIN_SpawnPoint.OnSpawn()
        /// A delay is required to allow for the end of dialogue system initialization
        /// </summary>
        public void HandleSpawn()
        {
            StartCoroutine(TriggerWithDelay());
        }

        private IEnumerator TriggerWithDelay()
        {
            if (_wasTriggered)
            {
                yield break;
            }
            _wasTriggered = true;
            
            yield return new WaitForSeconds(spawnDelay);
            Trigger();
        }

        public void Trigger()
        {
#if UNITY_EDITOR
            Debug.Log($"{nameof(DialogueTrigger)} triggered");
#endif

            DialoguePart dialogueToPlay = _dialogueSelector.SelectDialogueByFlag();
            DialogueSystem.Instance.StartDialogue(dialogueToPlay);
            DialogueSystem.Instance.onDialogueEnd.AddListener(HandleDialogueEnd);
        }

        private void HandleDialogueEnd()
        {
            _wasTriggered = false;
            DialogueSystem.Instance.onDialogueEnd.RemoveListener(HandleDialogueEnd);
        }
    }
}