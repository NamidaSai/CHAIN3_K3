using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Dialogue
{
    public class ChoiceDisplay : MonoBehaviour
    {
        [SerializeField] private TMP_Text choiceText;
        [SerializeField] private Button choiceButton;
        [SerializeField] private CanvasGroup canvasGroup;

        private Choice _choice;

        public void SetChoice(Choice choice)
        {
            _choice = choice;
            choiceText.text = choice.text;
            canvasGroup.alpha = 0f;
        }

        public void Show()
        {
            canvasGroup.interactable = true;
            canvasGroup.DOFade(1f, 0.3f);
        }

        private void Awake()
        {
            choiceButton.onClick.AddListener(HandleClick);
        }

        private void OnDestroy()
        {
            choiceButton.onClick.RemoveListener(HandleClick);
        }
        private void HandleClick()
        {
            DialogueSystem.Instance.HandleChoiceMade(_choice);
        }
    }
}