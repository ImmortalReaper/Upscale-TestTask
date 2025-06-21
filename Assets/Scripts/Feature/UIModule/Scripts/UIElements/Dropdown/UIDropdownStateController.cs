using System;
using System.Collections.Generic;
using Feature.AnimationModule.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Feature.UIModule.Scripts.UIElements.Dropdown
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class UIDropdownStateController : MonoBehaviour, 
        ISelectHandler, IDeselectHandler, 
        IPointerEnterHandler, IPointerExitHandler
    {
        [Header("CanvasGroups")]
        [SerializeField] private CanvasGroup normalGroup;
        [SerializeField] private CanvasGroup disabledGroup;
        [SerializeField] private CanvasGroup highlightedGroup;
    
        [Header("Highlight Animations (optional)")]
        [SerializeField] private bool useHighlightedAnimation = false;
        [SerializeField] private DOTweenSequenceAnimator highlightedIn;
        [SerializeField] private DOTweenSequenceAnimator highlightedOut;

        private TMP_Dropdown _dropdown;

        public TMP_Dropdown Dropdown => _dropdown;
        public Action<int> onDropdownValueChanged;

        private void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
            _dropdown.onValueChanged.AddListener(OnValueChanged);
        }

        private void OnEnable()
        {
            UpdateState();
        }

        private void OnDisable()
        {
            _dropdown.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(int index)
        {
            onDropdownValueChanged?.Invoke(index);
        }

        public void SetInteractable(bool interactable)
        {
            _dropdown.interactable = interactable;
            UpdateState();
        }

        private void UpdateState()
        {
            if (_dropdown.interactable)
                SetCanvasGroup(normalGroup);
            else
                SetCanvasGroup(disabledGroup);
        }

        private void SetCanvasGroup(CanvasGroup active)
        {
            List<CanvasGroup> groups = new List<CanvasGroup> { normalGroup, disabledGroup };
            if(!useHighlightedAnimation)
                groups.Add(highlightedGroup);
            foreach (var group in groups)
            {
                if(group == null) continue;
                bool isActive = group == active;
                group.alpha = isActive ? 1f : 0f;
                group.blocksRaycasts = isActive;
                group.interactable = isActive;
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            if(useHighlightedAnimation)
                highlightedIn?.PlaySequence();
            else
                SetCanvasGroup(highlightedGroup);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if(useHighlightedAnimation)
                highlightedOut?.PlaySequence();
            else
                SetCanvasGroup(normalGroup);
        }

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
    }
}
