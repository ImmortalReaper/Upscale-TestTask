using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class UISliderStateController : MonoBehaviour,
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
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI valueText;

    private Slider _slider;

    private void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.onValueChanged.AddListener(UpdateValueText);
    }

    private void OnEnable() => UpdateState();

    private void OnDestroy()
    {
        _slider.onValueChanged.RemoveListener(UpdateValueText);
    }

    private void UpdateValueText(float value)
    {
        if (valueText != null)
            valueText.text = value.ToString("F2");
    }
    
    public void SetInteractable(bool interactable)
    {
        _slider.interactable = interactable;
        UpdateState();
    }

    private void UpdateState()
    {
        if (!_slider.interactable)
            SetCanvasGroup(disabledGroup);
        else
            SetCanvasGroup(normalGroup);
    }

    private void SetCanvasGroup(CanvasGroup active)
    {
        List<CanvasGroup> groups = new List<CanvasGroup> { normalGroup, disabledGroup };
        
        if (!useHighlightedAnimation)
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
}