using System;
using UnityEngine;

namespace Feature.UIModule.Scripts.UIElements.Panel_Button
{
    [Serializable]
    public class UIPanelButton
    {
        [SerializeField] private UIPanellButtonStateController uiButton;
        [SerializeField] private CanvasGroup uiPanel;
    
        public UIPanellButtonStateController UIButton => uiButton;
        public CanvasGroup UIPanel => uiPanel;
    }
}
