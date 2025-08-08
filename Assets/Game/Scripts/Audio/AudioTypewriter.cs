using Game.Dialogue;
using UnityEngine;

namespace Game.Audio
{
    public class AudioTypewriter : MonoBehaviour
    {
        public void Trigger()
        {
            string soundName = DialogueSystem.Instance.GetCurrentSoundID();
            if (string.IsNullOrEmpty(soundName)) { return; }
            AudioManager.Instance.Play(soundName);
        }
    }
}