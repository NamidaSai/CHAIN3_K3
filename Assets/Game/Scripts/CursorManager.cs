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

        private bool _isDefault = true;
        private float _timeSinceChange = Mathf.Infinity;
        private float _currentCooldown = 1f;

        private void Awake()
        {
            Reset();
            interactSystem.onInteract.AddListener(HandleInteract);
            interactSystem.onHover.AddListener(HandleHover);
        }

        private void Reset()
        {
            _isDefault = true;
            Cursor.SetCursor(defaultSetting.texture, hotspot, cursorMode);
            _currentCooldown = defaultSetting.cooldown;
        }

        private void Update()
        {
            if (_isDefault) { return; }
            
            _timeSinceChange += Time.deltaTime;
            if (_timeSinceChange >= _currentCooldown)
            {
                Reset();
            }
        }

        private void HandleInteract(InteractSystem _)
        {
            _isDefault = false;
            _timeSinceChange = 0;
            Cursor.SetCursor(interactSetting.texture, hotspot, cursorMode);
            _currentCooldown = interactSetting.cooldown;
        }

        private void HandleHover()
        {
            if (!_isDefault) { return; }
            
            _isDefault = false;
            _timeSinceChange = 0;
            Cursor.SetCursor(hoverSetting.texture, hotspot, cursorMode);
            _currentCooldown = hoverSetting.cooldown;
        }
    }
}
