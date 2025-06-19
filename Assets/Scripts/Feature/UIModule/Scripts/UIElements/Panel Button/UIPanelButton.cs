using System;
using UnityEngine;

[Serializable]
public class UIPanelButton
{
    [SerializeField] private UIPanellButtonStateController uiButton;
    [SerializeField] private CanvasGroup uiPanel;
    
    public UIPanellButtonStateController UIButton => uiButton;
    public CanvasGroup UIPanel => uiPanel;
}
