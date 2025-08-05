namespace Game.Dialogue
{
    [System.Serializable]
    public struct DialogueLine
    {
        public string text;
        public Character character;
        public string soundID;
        public string animationTrigger;
    }
}