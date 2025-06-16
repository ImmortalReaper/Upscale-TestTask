using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIButtonStateController : MonoBehaviour, 
    IPointerEnterHandler, IPointerExitHandler, 
    ISelectHandler, IDeselectHandler
{
    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup normalGroup;
    [SerializeField] private CanvasGroup disabledGroup;
    [Header("Highlight Animations (optional)")]
    [SerializeField] private DOTweenSequenceAnimator highlightedIn;
    [SerializeField] private DOTweenSequenceAnimator highlightedOut;
    [Header("Selected Animations (optional)")]
    [SerializeField] private bool useHighlightedAnimation = false;
    [SerializeField] private DOTweenSequenceAnimator selectedIn;
    [SerializeField] private DOTweenSequenceAnimator selectedOut;

    private Button _button;

    public Button UIButton => _button;
    
    private void Awake() => _button = GetComponent<Button>();

    private void OnEnable() => UpdateState();

    public void OnPointerEnter(PointerEventData eventData) => highlightedIn?.PlaySequence();
    public void OnPointerExit(PointerEventData eventData) => highlightedOut?.PlaySequence();
    public void OnSelect(BaseEventData eventData)
    {
        if (useHighlightedAnimation)
            highlightedIn?.PlaySequence();
        else
            selectedIn?.PlaySequence();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (useHighlightedAnimation)
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
        CanvasGroup[] groups = { normalGroup, disabledGroup };
        foreach (var g in groups)
        {
            bool isActive = (g == active);
            g.alpha = isActive ? 1f : 0f;
            g.blocksRaycasts = isActive;
            g.interactable = isActive;
        }
    }
}