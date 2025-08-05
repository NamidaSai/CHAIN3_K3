using UnityEngine;

namespace Game.Interact
{
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void SetTrigger(string trigger)
        {
            if (animator != null)
            {
                animator.SetTrigger(trigger);
            }
        }
    }
}