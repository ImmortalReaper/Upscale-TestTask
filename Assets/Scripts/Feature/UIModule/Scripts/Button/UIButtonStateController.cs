using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class UIButtonStateController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ISelectHandler, IDeselectHandler
{
    public CanvasGroup normalGroup;
    public CanvasGroup disabledGroup;
    public DOTweenSequenceAnimator highlightedIn;
    public DOTweenSequenceAnimator highlightedOut;

    private Button _button;

    private void Awake() => _button = GetComponent<Button>();

    private void OnEnable() => UpdateState();

    public void OnPointerEnter(PointerEventData eventData)
    {
        //UpdateState();
        highlightedIn.PlaySequence();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //UpdateState();
        highlightedOut.PlaySequence();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateState();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpdateState();
    }

    public void OnSelect(BaseEventData eventData)
    {
        highlightedIn.PlaySequence();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        highlightedOut.PlaySequence();
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