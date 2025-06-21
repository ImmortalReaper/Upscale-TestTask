using System.Collections.Generic;
using Feature.AnimationModule.Scripts;
using UnityEngine;
using UnityEngine.EventSystems;
using CanvasGroup = UnityEngine.CanvasGroup;

namespace Feature.UIModule.Scripts.UIElements.Button
{
    [RequireComponent(typeof(UnityEngine.UI.Button))]
    public class UIButtonStateController : MonoBehaviour, 
        IPointerEnterHandler, IPointerExitHandler, 
        ISelectHandler, IDeselectHandler
    {
        [Header("Canvas Groups")]
        [SerializeField] private CanvasGroup normalGroup;
        [SerializeField] private CanvasGroup disabledGroup;
        [SerializeField] private CanvasGroup highlightedGroup;
        [Header("Highlight Animations (optional)")]
        [SerializeField] private bool useHighlightedAnimation = false;
        [SerializeField] private DOTweenSequenceAnimator highlightedIn;
        [SerializeField] private DOTweenSequenceAnimator highlightedOut;
        [Header("Selected Animations (optional)")]
        [SerializeField] private bool useHighlightedGroup = false;
        [SerializeField] private bool useCurrentHighlightedAnimation = false;
        [SerializeField] private DOTweenSequenceAnimator selectedIn;
        [SerializeField] private DOTweenSequenceAnimator selectedOut;

        private UnityEngine.UI.Button _button;

        public UnityEngine.UI.Button UIButton => _button;
    
        private void Awake() => _button = GetComponent<UnityEngine.UI.Button>();

        private void OnEnable() => UpdateState();

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(useHighlightedAnimation)
                highlightedIn?.PlaySequence();
            else
                SetCanvasGroup(highlightedGroup);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if(useHighlightedAnimation)
                highlightedOut?.PlaySequence();
            else
                SetCanvasGroup(normalGroup);
        }

        public void OnSelect(BaseEventData eventData)
        {
            if (useHighlightedGroup)
            {
                SetCanvasGroup(highlightedGroup);
                return;
            }
        
            if (useCurrentHighlightedAnimation)
                highlightedIn?.PlaySequence();
            else
                selectedIn?.PlaySequence();
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (useHighlightedGroup)
            {
                SetCanvasGroup(normalGroup);
                return;
            }
        
            if (useCurrentHighlightedAnimation)
                highlightedOut?.PlaySequence();
            else
                selectedOut?.PlaySequence();
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
            else
                SetCanvasGroup(normalGroup);
        }
    
        private void SetCanvasGroup(CanvasGroup active)
        {
            List<CanvasGroup> groups = new List<CanvasGroup> { normalGroup, disabledGroup, highlightedGroup };
        
            foreach (var g in groups)
            {
                if(g == null) continue;
                bool isActive = (g == active);
                g.alpha = isActive ? 1f : 0f;
                g.blocksRaycasts = isActive;
                g.interactable = isActive;
            }
        }
    }
}