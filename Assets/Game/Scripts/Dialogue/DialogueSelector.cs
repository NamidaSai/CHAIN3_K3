using System.Collections.Generic;
using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueSelector : MonoBehaviour
    {
        [Tooltip("Dialogues at the top of the list will always be picked first." +
                 "\nIf a dialogue has a flag condition, put at the top." +
                 "\nIf a dialogue does not have a flag, keep at bottom." +
                 "\nDoes not handle several dialogues with the same flag condition (null or otherwise)")]
        [SerializeField] private List<DialoguePartToFlagPair> dialoguePairs;

        /// <summary>
        /// Plays the first dialogue in the list whose flag has been set in the CHAIN_SharedData.
        /// Otherwise, plays the first dialogue whose flag is null or empty.
        /// </summary>
        public DialoguePart SelectDialogueByFlag()
        {
            foreach (DialoguePartToFlagPair pair in dialoguePairs)
            {
                if (string.IsNullOrEmpty(pair.flag)) { return pair.dialogue; }
                if (!CHAIN_SharedData.DoesFlagExist(pair.flag)) { continue; }
                return pair.dialogue;
            }

            Debug.LogError(
                $"{nameof(DialogueSelector)}.{nameof(SelectDialogueByFlag)}: No Dialogue found.");
            return null;
        }
    }
}