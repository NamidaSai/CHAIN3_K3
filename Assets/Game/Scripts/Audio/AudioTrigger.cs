using UnityEngine;

namespace Game.Audio
{
    public class AudioTrigger : MonoBehaviour
    {
        public void Trigger(string soundName)
        {
            AudioManager.Instance.Play(soundName);
        }
    }
}