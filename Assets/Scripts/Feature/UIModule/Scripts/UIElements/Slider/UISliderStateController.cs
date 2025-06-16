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

    [Header("Highlight Animations (optional)")]
    [SerializeField] private DOTweenSequenceAnimator highlightedIn;
    [SerializeField] private DOTweenSequenceAnimator highlightedOut;

    private Slider _slider;

    private void Awake() => _slider = GetComponent<Slider>();

    private void OnEnable() => UpdateState();

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
        foreach (var group in new[] { normalGroup, disabledGroup })
        {
            bool isActive = group == active;
            group.alpha = isActive ? 1f : 0f;
            group.blocksRaycasts = isActive;
            group.interactable = isActive;
        }
    }

    public void OnPointerEnter(PointerEventData eventData) => highlightedIn?.PlaySequence();
    public void OnPointerExit(PointerEventData eventData) => highlightedOut?.PlaySequence();
    public void OnSelect(BaseEventData eventData) => highlightedIn?.PlaySequence();
    public void OnDeselect(BaseEventData eventData) => highlightedOut?.PlaySequence();
}