using System;
using System.Collections;
using Game.Interact;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game
{
    [RequireComponent(typeof(Interactable))]
    public class EnterHandler : MonoBehaviour
    {
        [SerializeField] private float returnDelay = 1f;
        [FormerlySerializedAs("returnDuration")] [SerializeField] private float returnBlendTime = 3f;
        [SerializeField] private CinemachineBlendDefinition.Styles returnBlendStyle = CinemachineBlendDefinition.Styles.EaseInOut;

        private CinemachineBrain _brain;
        private CinemachineBlendDefinition _originalBlend;
        private Interactable _interactable;

        private void Awake()
        {
            _interactable = GetComponent<Interactable>();
        }

        public void HandleSpawn()
        {
            Camera mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Debug.LogError($"{nameof(EnterHandler)}.{nameof(HandleSpawn)}: Main Camera not found.");
                return;
            }
            
            _brain = mainCamera.GetComponent<CinemachineBrain>();
            _originalBlend = _brain.DefaultBlend;
            _brain.DefaultBlend = new CinemachineBlendDefinition(CinemachineBlendDefinition.Styles.Cut, 0);
            
            CinemachineVirtualCameraBase activeCamera = _brain.ActiveVirtualCamera as CinemachineVirtualCameraBase;
            if (activeCamera != null) { activeCamera.gameObject.SetActive(false); }
            _interactable.ActivateCamera(true);
            _interactable.CanBeInteractedWith = false;
            InteractSystem.Instance.CanInteract = false;
            
            _brain.DefaultBlend = new CinemachineBlendDefinition(returnBlendStyle, returnBlendTime);

            StartCoroutine(ReturnAfterDelay());
        }

        private IEnumerator ReturnAfterDelay()
        {
            yield return new WaitForSeconds(returnDelay);
            
            _interactable.ActivateCamera(false);
            InteractSystem.Instance.Return();
            
            yield return new WaitForSeconds(returnBlendTime);
            
            _interactable.CanBeInteractedWith = true;
            InteractSystem.Instance.CanInteract = true;
            _brain.DefaultBlend = _originalBlend;
        }
    }
}