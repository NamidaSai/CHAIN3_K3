using UnityEngine;

namespace Game.Dialogue
{
    [System.Serializable]
    public struct Choice
    {
        [Tooltip("Text displayed on the Choice Display button")]
        public string text;
        [Tooltip("Automatically starts this Dialogue Part if Choice selected." +
                 "\nLeave empty to close dialogue.")]
        public DialoguePart nextPart;
        [Tooltip("Choice will only display if Chain Shared Data Flag has been set" +
                 "\nLeave empty to always show this choice.")]
        public string flagRequired;
        [Tooltip("Selecting this Choice will create a new Chain Shared Data Flag" +
                 "\nLeave empty to not create a flag.")]
        public string flagToCreate;
    }
}