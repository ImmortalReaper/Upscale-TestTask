using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDropdownToggleStateController : MonoBehaviour,
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
    [SerializeField] private TextMeshProUGUI textNormal;
    [SerializeField] private TextMeshProUGUI textDisabled;
    [SerializeField] private TextMeshProUGUI textHighlighted;
    [Header("Checkmark")]
    [SerializeField] private Image checkmarkNormal;
    [SerializeField] private Image checkmarkDisabled;
    [SerializeField] private Image checkmarkHighlighted;
    
    private Toggle _toggle;
    
    public event Action OnHiglighted;
    public event Action OnSelected;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(HandleToggleValueChanged);
    }

    private void Start()
    {
        UpdateGroupState();
        UpdateAllStates();
    }

    private void OnEnable()
    {
        UpdateGroupState();
        UpdateAllStates();
    }

    private void OnDisable()
    {
        _toggle.onValueChanged.RemoveListener(HandleToggleValueChanged);
    }

    private void HandleToggleValueChanged(bool isOn)
    {
        UpdateGroupState();
        UpdateAllStates();
    }

    private void UpdateAllStates()
    {
        if (textHighlighted && checkmarkHighlighted)
        {
            textHighlighted.text = textNormal.text;
            checkmarkHighlighted.enabled = _toggle.isOn;
        }
        if(textDisabled && checkmarkDisabled)
        {
            textDisabled.text = textNormal.text;
            checkmarkDisabled.enabled = _toggle.isOn;
        }
        if (checkmarkNormal)
        {
            checkmarkNormal.enabled = _toggle.isOn;
        }
    }
    
    private void UpdateGroupState()
    {
        if (_toggle.interactable)
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
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(useHighlightedAnimation)
            highlightedIn?.PlaySequence();
        else
            SetCanvasGroup(highlightedGroup);
        OnHiglighted?.Invoke();
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
        OnSelected?.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if(useHighlightedAnimation)
            highlightedOut?.PlaySequence();
        else
            SetCanvasGroup(normalGroup);
    }

    public void SetInteractable(bool interactable)
    {
        _toggle.interactable = interactable;
        UpdateGroupState();
    }
}
