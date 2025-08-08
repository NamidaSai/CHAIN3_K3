using System.Collections;
using Game.Interact;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Exit : MonoBehaviour
    {
        [SerializeField] private string targetDoorId;
        [SerializeField] private float delay = 1f;

        private bool _wasTriggered = false;

        public UnityEvent<string> onExit;
        
        public void Trigger()
        {
            if (_wasTriggered) { return; }
            _wasTriggered = true;
            GetComponent<Interactable>().CanBeInteractedWith = false;
            StartCoroutine(ExitAfterDelay());
        }

        private IEnumerator ExitAfterDelay()
        {
            onExit?.Invoke(targetDoorId);
            yield return new WaitForSeconds(delay);
#if UNITY_EDITOR
            Debug.Log("Exiting via door " + targetDoorId);
#endif
            CHAIN_SceneSelector.ExitGame(targetDoorId);
        }
    }
}