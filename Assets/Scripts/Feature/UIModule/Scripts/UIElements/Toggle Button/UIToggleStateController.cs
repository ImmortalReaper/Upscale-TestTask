using Core.Audio.Scripts;
using Feature.AnimationModule.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Feature.UIModule.Scripts.UIElements.Toggle_Button
{
    [RequireComponent(typeof(Toggle))]
    public class UIToggleStateController : MonoBehaviour,
        IPointerEnterHandler, IPointerExitHandler,
        ISelectHandler, IDeselectHandler
    {
        [Header("Canvas Groups")]
        [SerializeField] private CanvasGroup onGroup;
        [SerializeField] private CanvasGroup offGroup;
        [SerializeField] private CanvasGroup highlightedGroup;
        [Header("Toggle-State Animations")]
        [SerializeField] private bool useOnOffAnimations = false;
        [SerializeField] private DOTweenSequenceAnimator onStateAnim;
        [SerializeField] private DOTweenSequenceAnimator offStateAnim;
        [Header("Highlight Animations (optional)")]
        [SerializeField] private DOTweenSequenceAnimator highlightedIn;
        [SerializeField] private DOTweenSequenceAnimator highlightedOut;

        private IAudioService _audioService;
        private Toggle _toggle;

        [Inject]
        public void InjectDependencies(IAudioService audioService)
        {
            _audioService = audioService;
        }
        
        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(HandleToggleValueChanged);
        }

        private void OnEnable()
        {
            UpdateVisualState(_toggle.isOn, false);
            DisableHighlight();
        }

        private void OnDestroy()
        {
            _toggle.onValueChanged.RemoveListener(HandleToggleValueChanged);
        }

        private void HandleToggleValueChanged(bool isOn)
        {
            _audioService.PlayOneShot(_audioService.SFXConfig.ClickUI, Vector3.zero);
            UpdateVisualState(isOn, true);
        }

        private void UpdateVisualState(bool isOn, bool playAnim)
        {
            if (useOnOffAnimations && playAnim)
            {
                if (isOn)  
                    onStateAnim.PlaySequence();
                else       
                    offStateAnim.PlaySequence();
                return;
            }
        
            CanvasGroup activeGroup = isOn ? onGroup : offGroup;
            CanvasGroup inactiveGroup = isOn ? offGroup : onGroup;
        
            activeGroup.alpha = 1f;
            activeGroup.interactable = true;
            activeGroup.blocksRaycasts = true;

            inactiveGroup.alpha = 0f;
            inactiveGroup.interactable = false;
            inactiveGroup.blocksRaycasts = false;
        }

        private void DisableHighlight()
        {
            highlightedGroup.alpha = 0f;
            highlightedGroup.interactable = false;
            highlightedGroup.blocksRaycasts = false;
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            highlightedIn?.PlaySequence();
            _audioService.PlayOneShot(_audioService.SFXConfig.HoverUI, Vector3.zero);
        }
        public void OnPointerExit(PointerEventData eventData) => highlightedOut?.PlaySequence();
        public void OnSelect(BaseEventData eventData)
        {
            highlightedIn?.PlaySequence();
            _audioService.PlayOneShot(_audioService.SFXConfig.HoverUI, Vector3.zero);
        }
        public void OnDeselect(BaseEventData eventData) => highlightedOut?.PlaySequence();
    
        public void SetInteractable(bool interactable)
        {
            _toggle.interactable = interactable;
            UpdateVisualState(_toggle.isOn, false);
        }
    }
}
