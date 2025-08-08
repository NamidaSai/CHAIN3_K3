using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.Dialogue
{
    public class DialogueDisplay : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_Text characterDisplay;
        [SerializeField] private TMP_Text lineDisplay;
        [SerializeField] private RectTransform choiceContainer;
        [SerializeField] private GameObject choicePrefab;
        [SerializeField] private CanvasGroup canvasGroup;
        
        [SerializeField] private RectTransform debugContainer;
        [SerializeField] private TMP_Text debugDisplay;
        [SerializeField] private bool showDebug = true;

        private readonly List<GameObject> _currentChoices = new();

        private float _startAnchoredY;
        
        private const float TweenDuration = 0.5f;

        public void HandleTypewriterEnd()
        {
            StartCoroutine(ShowChoicesAfterDelay());
        }

        private IEnumerator ShowChoicesAfterDelay()
        {
            yield return new WaitForSeconds(0.5f);
            foreach (GameObject choice in _currentChoices)
            {
                ChoiceDisplay choiceDisplay = choice.GetComponent<ChoiceDisplay>();
                if (choiceDisplay) { choiceDisplay.Show(); }
            }
        }

        // Dialogue System is initialised in Awake()
        private void Start()
        {
            DialogueSystem.Instance.onPlayLine.AddListener(HandlePlayLine);
            DialogueSystem.Instance.onDisplayChoices.AddListener(HandleDisplayChoices);
            DialogueSystem.Instance.onDialogueStart.AddListener(HandleStartDialogue);
            DialogueSystem.Instance.onDialogueEnd.AddListener(HandleEndDialogue);
            
            canvas.gameObject.SetActive(false);
            _startAnchoredY = choiceContainer.anchoredPosition.y;
            canvasGroup.alpha = 0f;

#if !UNITY_EDITOR
            debugContainer.gameObject.SetActive(false);
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            debugContainer.gameObject.SetActive(showDebug);
        }
#endif
        
        private void OnDestroy()
        {
            DialogueSystem.Instance.onPlayLine.RemoveListener(HandlePlayLine);
            DialogueSystem.Instance.onDisplayChoices.RemoveListener(HandleDisplayChoices);
            DialogueSystem.Instance.onDialogueStart.RemoveListener(HandleStartDialogue);
            DialogueSystem.Instance.onDialogueEnd.RemoveListener(HandleEndDialogue);
        }

        private void HandleStartDialogue()
        {
            if (!canvas.gameObject.activeSelf)
            {
                canvas.gameObject.SetActive(true);
                choiceContainer.anchoredPosition = new Vector2(choiceContainer.anchoredPosition.x, _startAnchoredY);
                choiceContainer.DOAnchorPosY(10f, TweenDuration)
                    .From()
                    .SetEase(Ease.OutSine);
                canvasGroup.DOFade(1f, TweenDuration);
            }
            
#if UNITY_EDITOR
            debugDisplay.text = DialogueSystem.Instance.GetCurrentDialoguePart();
#endif
        }

        private void HandleEndDialogue()
        {
            StartCoroutine(DisableAfterDelay());
        }

        private IEnumerator DisableAfterDelay()
        {
            choiceContainer.DOAnchorPosY(10f, TweenDuration)
                .SetEase(Ease.OutSine);
            canvasGroup.DOFade(0f, TweenDuration);
            yield return new WaitForSeconds(TweenDuration);
            canvas.gameObject.SetActive(false);
        }

        private void HandlePlayLine(DialogueLine lineToPlay)
        {
            string characterName = lineToPlay.character.ToString().ToUpper();
            characterDisplay.text = $"{characterName}";
            lineDisplay.text = lineToPlay.text;
        }

        private void HandleDisplayChoices(List<Choice> choicesToDisplay)
        {
            ClearCurrentChoices();
            
            foreach (Choice choice in choicesToDisplay)
            {
                GameObject newChoiceObject = Instantiate(choicePrefab, choiceContainer);
                _currentChoices.Add(newChoiceObject);
                ChoiceDisplay newChoiceDisplay = newChoiceObject.GetComponent<ChoiceDisplay>();
                newChoiceDisplay.SetChoice(choice);
            }
        }
        
        private void ClearCurrentChoices()
        {
            if (_currentChoices.Count <= 0) { return; }
            
            foreach (GameObject choiceGameObject in _currentChoices)
            {
                Destroy(choiceGameObject);
            }

            _currentChoices.Clear();
        }
    }
}