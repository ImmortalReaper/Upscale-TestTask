using System.Collections.Generic;
using UnityEngine;

public class UIButtonsPanelCobtroller : MonoBehaviour
{
    [SerializeField] private List<UIPanelButton> uiButtons = new();
    
    private int _currentIndex = -1;

    private void Awake()
    {
        for (int i = 0; i < uiButtons.Count; i++)
        {
            int idx = i;
            uiButtons[i].UIOnButtonClick.AddListener(() => OnButtonClicked(idx));
        }
    }

    private void Start()
    {
        if (uiButtons.Count > 0)
            OnButtonClicked(0);
    }

    private void OnButtonClicked(int index)
    {
        if (index < 0 || index >= uiButtons.Count || index == _currentIndex)
            return;

        _currentIndex = index;
        RefreshAll();
    }

    private void RefreshAll()
    {
        for (int i = 0; i < uiButtons.Count; i++)
        {
            bool isActive = (i == _currentIndex);
            
            SetCanvasGroup(uiButtons[i].UIPanel, isActive);
            uiButtons[i].UIButton.SetInteractable(!isActive);
        }
    }

    private void SetCanvasGroup(CanvasGroup cg, bool show)
    {
        cg.alpha = show ? 1f : 0f;
        cg.interactable = show;
        cg.blocksRaycasts = show;
    }
}
