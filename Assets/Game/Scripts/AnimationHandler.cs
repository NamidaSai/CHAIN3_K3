using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class AnimationHandler : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        [HideInInspector]
        public UnityEvent onInteractEnd;
        
        public void SetTrigger(string trigger)
        {
            if (animator != null)
            {
                animator.SetTrigger(trigger);
            }
        }

        // triggered by animation event on Interact animation
        public void OnInteractEnd()
        {
            onInteractEnd?.Invoke();
        }
    }
}