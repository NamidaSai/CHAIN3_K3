using Game.Interact;
using UnityEngine;

namespace Game.Audio
{
    public class AudioInteractableHover : MonoBehaviour
    {
        public void Trigger(Interactable interactable)
        {
            string soundToPlay = interactable.GetHoverSoundID();
            if (string.IsNullOrEmpty(soundToPlay)) { return; }
            AudioManager.Instance.Play(soundToPlay);
        }
    }
}