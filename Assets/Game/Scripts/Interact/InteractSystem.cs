using DG.Tweening;
using Game.Dialogue;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Interact
{
    public class InteractSystem : MonoBehaviour
    {
        public static InteractSystem Instance;
        
        [SerializeField] private CinemachineCamera primaryCamera;
        [SerializeField] private float maxInteractDistance = 5f;
        [SerializeField] private CanvasGroup returnCanvas;
        [SerializeField] private RectTransform returnRect;
        
        public bool CanInteract { private get; set; } = true;
        public bool IsBlocked { private get; set; } = true;

        private Camera _mainCamera;
        private InputAction _interactAction;
        private InputAction _returnAction;

        public UnityEvent<InteractSystem> onInteract;
        public UnityEvent<InteractSystem> onReturn;
        public UnityEvent<Interactable> onHoverEnter;
        public UnityEvent<Interactable> onHoverExit;

        private Interactable _currentHovered;

        private float _startReturnAnchoredY;
        
        public void Return()
        {
            if (!IsBlocked) { return; }
            onReturn?.Invoke(this);
            DialogueSystem.Instance.EndDialogue();
            primaryCamera.gameObject.SetActive(true);
            
            returnCanvas.DOFade(0f, 0.5f);
            returnRect.DOAnchorPosY(10f, 0.5f)
                .SetEase(Ease.OutSine);
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            
            _mainCamera = Camera.main;

            _interactAction = InputSystem.actions.FindAction("Interact");
            _returnAction = InputSystem.actions.FindAction("Return");

            _interactAction.performed += HandleInteract;
            _returnAction.performed += HandleReturn;
        }

        private void Start()
        {
            if (DialogueSystem.Instance == null)
            {
                Debug.LogError($"{nameof(InteractSystem)}.{nameof(Start)}: Dialogue System not found.");
                return;
            }
            
            DialogueSystem.Instance.onDialogueStart.AddListener(HandleDialogueStart);
            DialogueSystem.Instance.onDialogueEnd.AddListener(HandleDialogueEnd);

            returnCanvas.alpha = 0f;
            _startReturnAnchoredY = returnRect.anchoredPosition.y;
        }

        private void OnDestroy()
        {
            _interactAction.performed -= HandleInteract;
            _returnAction.performed -= HandleReturn;
            
            if (DialogueSystem.Instance == null)
            {
                return;
            }
            
            DialogueSystem.Instance.onDialogueStart.RemoveListener(HandleDialogueStart);
            DialogueSystem.Instance.onDialogueEnd.RemoveListener(HandleDialogueEnd);
        }

        private void HandleDialogueStart()
        {
            CanInteract = false;

            if (_currentHovered == null) { return; }
            onHoverExit?.Invoke(_currentHovered);
            _currentHovered.OnHoverExit(this);
            _currentHovered = null;
        }

        private void HandleDialogueEnd()
        {
            CanInteract = true;
        }

        private void Update()
        {
            if (!IsBlocked) { return; }
            if (!CanInteract) { return; }
            
            Interactable newHover = GetInteractableUnderCursor();
            if (newHover == _currentHovered) { return; }
            
            if (_currentHovered)
            {
                onHoverExit?.Invoke(_currentHovered);
                _currentHovered.OnHoverExit(this);
            }

            if (newHover)
            {
                onHoverEnter?.Invoke(newHover);
                newHover.OnHoverEnter(this);
            }

            _currentHovered = newHover;
        }

        private void HandleInteract(InputAction.CallbackContext context)
        {
            if (_currentHovered == null) { return; }
            Interact(_currentHovered);
        }

        private void HandleReturn(InputAction.CallbackContext context)
        {
            Return();
        }

        private void Interact(Interactable target)
        {
            onInteract?.Invoke(this);
            target.Interact(this);
            primaryCamera.gameObject.SetActive(false);
            
            returnCanvas.DOFade(1f, 0.5f);
            returnRect.anchoredPosition = new Vector2(returnRect.anchoredPosition.x, _startReturnAnchoredY);
            returnRect.DOAnchorPosY(10f, 0.5f)
                .From()
                .SetEase(Ease.OutSine);
        }

        private Interactable GetInteractableUnderCursor()
        {
            Ray ray = _mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (!Physics.Raycast(ray, out RaycastHit hit, maxInteractDistance)) { return null; }
            
            GameObject hitObject = hit.collider.gameObject;
            return hitObject.CompareTag("Interactable") 
                ? hitObject.GetComponent<Interactable>() 
                : null;
        }
    }
}