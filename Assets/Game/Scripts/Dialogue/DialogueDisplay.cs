using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Dialogue
{
    public class DialogueDisplay : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private TMP_Text characterDisplay;
        [SerializeField] private TMP_Text lineDisplay;
        [SerializeField] private RectTransform choiceContainer;
        [SerializeField] private CanvasGroup canvasGroup;

        [Header("Prefabs")]
        [Tooltip("Prefab that contains a ChoiceDisplay component on the root.")]
        [SerializeField] private ChoiceDisplay choiceDisplayPrefab;

        [Header("Pooling")]
        [Tooltip("Initial pool capacity. Not a hard limit.")]
        [SerializeField] private int initialPoolCapacity = 4;
        [Tooltip("Absolute max pooled instances. Extra are destroyed when released.")]
        [SerializeField] private int maxPoolSize = 32;

        [Header("Debug")]
        [SerializeField] private RectTransform debugContainer;
        [SerializeField] private TMP_Text debugDisplay;
        [SerializeField] private bool showDebug = true;

        private readonly List<ChoiceDisplay> _activeChoiceDisplays = new();

        private float _choiceContainerStartY;
        private const float TweenDuration = 0.5f;

        private ObjectPool<ChoiceDisplay> _choiceDisplayPool;

        public void HandleTypewriterEnd()
        {
            StartCoroutine(RevealChoicesAfterDelay());
        }

        private IEnumerator RevealChoicesAfterDelay()
        {
            yield return new WaitForSeconds(0.5f);
            foreach (ChoiceDisplay choiceDisplay in _activeChoiceDisplays)
            {
                if (choiceDisplay != null)
                {
                    choiceDisplay.Show();
                }
            }
        }

        private void Awake()
        {
            _choiceDisplayPool = new ObjectPool<ChoiceDisplay>(
                createFunc: CreateChoiceDisplayInstance,
                actionOnGet: OnGetChoiceDisplay,
                actionOnRelease: OnReleaseChoiceDisplay,
                actionOnDestroy: OnDestroyChoiceDisplay,
                collectionCheck: false,
                defaultCapacity: initialPoolCapacity,
                maxSize: maxPoolSize
            );
        }

        // Dialogue System is initialised in Awake()
        private void Start()
        {
            DialogueSystem.Instance.onPlayLine.AddListener(HandlePlayLine);
            DialogueSystem.Instance.onDisplayChoices.AddListener(HandleDisplayChoices);
            DialogueSystem.Instance.onDialogueStart.AddListener(HandleStartDialogue);
            DialogueSystem.Instance.onDialogueEnd.AddListener(HandleEndDialogue);

            canvas.gameObject.SetActive(false);
            _choiceContainerStartY = choiceContainer.anchoredPosition.y;
            canvasGroup.alpha = 0f;

#if !UNITY_EDITOR
            if (debugContainer != null) debugContainer.gameObject.SetActive(false);
#endif
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (debugContainer != null)
            {
                debugContainer.gameObject.SetActive(showDebug);
            }
        }
#endif

        private void OnDestroy()
        {
            if (DialogueSystem.Instance != null)
            {
                DialogueSystem.Instance.onPlayLine.RemoveListener(HandlePlayLine);
                DialogueSystem.Instance.onDisplayChoices.RemoveListener(HandleDisplayChoices);
                DialogueSystem.Instance.onDialogueStart.RemoveListener(HandleStartDialogue);
                DialogueSystem.Instance.onDialogueEnd.RemoveListener(HandleEndDialogue);
            }

            ReleaseAllActiveChoices();
        }

        private void HandleStartDialogue()
        {
            if (!canvas.gameObject.activeSelf)
            {
                canvas.gameObject.SetActive(true);
                choiceContainer.anchoredPosition = new Vector2(choiceContainer.anchoredPosition.x, _choiceContainerStartY);
                choiceContainer.DOAnchorPosY(10f, TweenDuration)
                    .From()
                    .SetEase(Ease.OutSine);
                canvasGroup.DOFade(1f, TweenDuration);
            }

#if UNITY_EDITOR
            if (debugDisplay != null)
            {
                debugDisplay.text = DialogueSystem.Instance.GetCurrentDialoguePart();
            }
#endif
        }

        private void HandleEndDialogue()
        {
            StartCoroutine(FadeOutAndDisableAfterDelay());
        }

        private IEnumerator FadeOutAndDisableAfterDelay()
        {
            choiceContainer.DOAnchorPosY(10f, TweenDuration)
                .SetEase(Ease.OutSine);
            canvasGroup.DOFade(0f, TweenDuration);
            yield return new WaitForSeconds(TweenDuration);
            canvas.gameObject.SetActive(false);
        }

        private void HandlePlayLine(DialogueLine line)
        {
            var characterName = line.character.ToString().ToUpperInvariant();
            characterDisplay.text = characterName;
            lineDisplay.text = line.text;
        }

        private void HandleDisplayChoices(List<Choice> choices)
        {
            ReleaseAllActiveChoices();

            foreach (Choice choice in choices)
            {
                ChoiceDisplay choiceDisplay = _choiceDisplayPool.Get();
                choiceDisplay.transform.SetParent(choiceContainer, false);
                choiceDisplay.SetChoice(choice);
                _activeChoiceDisplays.Add(choiceDisplay);
            }
        }

        private void ReleaseAllActiveChoices()
        {
            if (_activeChoiceDisplays.Count == 0) return;

            foreach (ChoiceDisplay choiceDisplay in _activeChoiceDisplays)
            {
                if (choiceDisplay != null)
                {
                    _choiceDisplayPool.Release(choiceDisplay);
                }
            }

            _activeChoiceDisplays.Clear();
        }

        // Pool hooks
        private ChoiceDisplay CreateChoiceDisplayInstance()
        {
            ChoiceDisplay instance = Instantiate(choiceDisplayPrefab, choiceContainer);
            instance.gameObject.SetActive(false);
            return instance;
        }

        private void OnGetChoiceDisplay(ChoiceDisplay choiceDisplay)
        {
            choiceDisplay.gameObject.SetActive(true);
            choiceDisplay.PrepareForReuse();
        }

        private void OnReleaseChoiceDisplay(ChoiceDisplay choiceDisplay)
        {
            choiceDisplay.PrepareForReuse();
            choiceDisplay.gameObject.SetActive(false);
        }

        private void OnDestroyChoiceDisplay(ChoiceDisplay choiceDisplay)
        {
            if (choiceDisplay == null) return;
            Destroy(choiceDisplay.gameObject);
        }
    }
}
