using UnityEngine;

namespace Game.Dialogue
{
    [System.Serializable]
    public struct DialoguePartToFlagPair
    {
        [Tooltip("Dialogue Part to Play")]
        public DialoguePart dialogue;
        [Tooltip("Required Chain Shared Data Flag")]
        public string flag;
    }
}