using UnityEngine;

namespace Game.Audio
{
    public class AudioTrigger : MonoBehaviour
    {
        public void Trigger(string soundName)
        {
            if (string.IsNullOrEmpty(soundName)) { return; }
            AudioManager.Instance.Play(soundName);
        }
    }
}