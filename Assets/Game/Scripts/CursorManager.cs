using UnityEngine;

namespace Game
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

        private bool _isInCooldown = false;
        private bool _isHovering = false;
        private float _cooldownTimer = Mathf.Infinity;

        private void Awake()
        {
            SetCursor(defaultSetting.texture);

            interactSystem.onInteract.AddListener(HandleInteract);
            interactSystem.onHoverEnter.AddListener(_ => HandleHoverEnter());
            interactSystem.onHoverExit.AddListener(_ => HandleHoverExit());
        }

        private void Update()
        {
            if (!_isInCooldown) { return; }

            _cooldownTimer += Time.deltaTime;
            if (_cooldownTimer < interactSetting.cooldown) { return; }
            _isInCooldown = false;
            _cooldownTimer = Mathf.Infinity;

            SetCursor(_isHovering ? hoverSetting.texture : defaultSetting.texture);
        }

        private void HandleInteract(InteractSystem _)
        {
            SetCursor(interactSetting.texture);
            _isInCooldown = true;
            _cooldownTimer = 0f;
        }

        private void HandleHoverEnter()
        {
            _isHovering = true;
            if (_isInCooldown) { return; }
            SetCursor(hoverSetting.texture);
        }

        private void HandleHoverExit()
        {
            _isHovering = false;
            if (_isInCooldown) { return; }
            SetCursor(defaultSetting.texture);
        }

        private void SetCursor(Texture2D texture)
        {
            Cursor.SetCursor(texture, hotspot, cursorMode);
        }
    }
}