using System.Collections.Generic;

namespace Game.Dialogue
{
    [System.Serializable]
    public struct Choice
    {
        public string text;
        public DialoguePart nextPart;
        public List<string> flagsToCreate;
    }
}