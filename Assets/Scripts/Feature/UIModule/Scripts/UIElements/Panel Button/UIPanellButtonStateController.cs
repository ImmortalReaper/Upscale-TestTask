using System.Collections.Generic;
using Core.Audio.Scripts;
using Feature.AnimationModule.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Feature.UIModule.Scripts.UIElements.Panel_Button
{
    [RequireComponent(typeof(Button))]
    public class UIPanellButtonStateController : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Canvas Groups")]
        [SerializeField] private CanvasGroup normalGroup;
        [SerializeField] private CanvasGroup disabledGroup;
        [SerializeField] private CanvasGroup highlightedGroup;
        [SerializeField] private CanvasGroup selectedGroup;
        [Header("Highlight Settings")]
        public bool useHighlightAnimation = false;
        public DOTweenSequenceAnimator highlightedIn;
        public DOTweenSequenceAnimator highlightedOut;
        [Header("Selection Settings")]
        public bool useHighlightedGroupForSelection = false;
        public bool useHighlightAnimationForSelection = false;
        public bool useSelectedAnimations = false;
        public DOTweenSequenceAnimator selectedIn;
        public DOTweenSequenceAnimator selectedOut;

        private IAudioService _audioService;
        private Button _button;
        private bool _isSelected = false;

        public Button Button => _button;
        public bool IsSelected => _isSelected;
    
        [Inject]
        public void InjectDependencies(IAudioService audioService)
        {
            _audioService = audioService;
        }
        
        private void Awake() => _button = GetComponent<Button>();

        private void OnEnable()
        {
            UpdateState();
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_isSelected) return;
            if(useHighlightAnimation)
                highlightedIn?.PlaySequence();
            else
                SetCanvasGroup(highlightedGroup);
            _audioService.PlayOneShot(_audioService.SFXConfig.HoverUI, Vector3.zero);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_isSelected) return;
            if(useHighlightAnimation)
                highlightedIn?.PlaySequence();
            else
                SetCanvasGroup(normalGroup);
        }

        public void DeselectButton()
        {
            if(!_isSelected) return;
        
            if (useHighlightedGroupForSelection)
            {
                SetCanvasGroup(normalGroup);
                _isSelected = false;
                return;
            }
        
            if (useHighlightAnimationForSelection)
                highlightedOut?.PlaySequence();
            else if (useSelectedAnimations)
                selectedOut?.PlaySequence();
            else
                SetCanvasGroup(normalGroup);
        
            _isSelected = false;
        }
    
        public void SelectButton()
        {
            if(_isSelected) return;
            _audioService.PlayOneShot(_audioService.SFXConfig.ClickUI, Vector3.zero);
            if (useHighlightedGroupForSelection)
            {
                SetCanvasGroup(highlightedGroup);
                _isSelected = true;
                return;
            }

            if (useHighlightAnimationForSelection)
                highlightedIn?.PlaySequence();
            else if(useSelectedAnimations)
                selectedIn?.PlaySequence();
            else
                SetCanvasGroup(selectedGroup);
            _isSelected = true;
        }
    
        public void OnClick()
        {
            _audioService.PlayOneShot(_audioService.SFXConfig.ClickUI, Vector3.zero);
        }
        
        public void SetInteractable(bool interactable)
        {
            _button.interactable = interactable;
            UpdateState();
        }

        private void UpdateState()
        {
            if (!_button.interactable)
                SetCanvasGroup(disabledGroup);
            else if(_isSelected)
                SetCanvasGroup(selectedGroup);
            else
                SetCanvasGroup(normalGroup);
        }
    
        private void SetCanvasGroup(CanvasGroup active)
        {
            List<CanvasGroup> groups = new List<CanvasGroup> { normalGroup, disabledGroup };
        
            if (!useHighlightAnimation)
                groups.Add(highlightedGroup);
        
            if (!useSelectedAnimations)
                groups.Add(selectedGroup);
        
            foreach (var group in groups)
            {
                if(group == null) continue;
                bool isActive = (group == active);
                group.alpha = isActive ? 1f : 0f;
                group.blocksRaycasts = isActive;
                group.interactable = isActive;
            }
        }
    }
}
