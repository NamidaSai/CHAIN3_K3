using System.Collections;
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