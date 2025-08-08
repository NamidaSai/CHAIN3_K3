using UnityEngine;

namespace Game.Interact
{
    public class CursorManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private InteractSystem interactSystem;

        [Header("Default Settings")]
        [SerializeField] private Vector2 hotspot;
        [SerializeField] private CursorMode cursorMode;
        [SerializeField] private CursorSetting defaultSetting;

        [Header("Custom Settings")]
        [SerializeField] private CursorSetting hoverSetting;
        [SerializeField] private CursorSetting interactSetting;
        [SerializeField] private CursorSetting exitHoverSetting;
        [SerializeField] private CursorSetting exitInteractSetting;
        [SerializeField] private CursorSetting talkHoverSetting;
        [SerializeField] private CursorSetting talkInteractSetting;

        private bool _isInCooldown = false;
        private bool _isHovering = false;
        private bool _isHoveringExit = false;
        private bool _isHoveringSlater = false;
        private float _cooldownTimer = Mathf.Infinity;

        private void Awake()
        {
            SetCursor(defaultSetting.texture);

            interactSystem.onInteract.AddListener(HandleInteract);
            interactSystem.onHoverEnter.AddListener(HandleHoverEnter);
            interactSystem.onHoverExit.AddListener(_ => HandleHoverExit());
        }

        private void Update()
        {
            if (!_isInCooldown) { return; }

            _cooldownTimer += Time.deltaTime;
            if (_cooldownTimer < interactSetting.cooldown) { return; }
            _isInCooldown = false;
            _cooldownTimer = Mathf.Infinity;

            if (_isHovering)
            {
                if (_isHoveringExit)
                {
                    SetCursor(exitHoverSetting.texture);
                }
                else if (_isHoveringSlater)
                {
                    SetCursor(talkHoverSetting.texture);
                }
                else
                {
                    SetCursor(hoverSetting.texture);
                }
            }
            else
            {
                SetCursor(defaultSetting.texture);
            }
        }

        private void HandleInteract(InteractSystem _)
        {
            if (_isHoveringExit)
            {
                SetCursor(exitInteractSetting.texture);
            }
            else if (_isHoveringSlater)
            {
                SetCursor(talkInteractSetting.texture);
            }
            else
            {
                SetCursor(interactSetting.texture);
            }
            
            _isInCooldown = true;
            _cooldownTimer = 0f;
        }

        private void HandleHoverEnter(Interactable interactable)
        {
            _isHovering = true;
            if (_isInCooldown) { return; }

            if (interactable.GetComponent<Exit>())
            {
                _isHoveringExit = true;
                SetCursor(exitHoverSetting.texture);
                return;
            }

            if (interactable.GetComponent<Slater>())
            {
                _isHoveringSlater = true;
                SetCursor(talkHoverSetting.texture);
                return;
            }

            SetCursor(hoverSetting.texture);
        }

        private void HandleHoverExit()
        {
            _isHovering = false;
            _isHoveringExit = false;
            _isHoveringSlater = false;
            if (_isInCooldown) { return; }
            SetCursor(defaultSetting.texture);
        }

        private void SetCursor(Texture2D texture)
        {
            Cursor.SetCursor(texture, hotspot, cursorMode);
        }
    }
}