using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UIPanelButton
{
    [SerializeField] private UIButtonStateController uiButton;
    [SerializeField] private CanvasGroup uiPanel;
    
    public UIButtonStateController UIButton => uiButton;
    public Button.ButtonClickedEvent UIOnButtonClick => uiButton.UIButton.onClick;
    public CanvasGroup UIPanel => uiPanel;
}
