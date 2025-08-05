using System.Collections.Generic;
using UnityEngine;

namespace Game.Dialogue
{
    [CreateAssetMenu(fileName = "DialoguePart", menuName = "CHAIN/DialoguePart", order = 0)]
    public class DialoguePart : ScriptableObject
    {
        public List<DialogueLine> dialogueLines;
        public List<Choice> choices;
    }
}