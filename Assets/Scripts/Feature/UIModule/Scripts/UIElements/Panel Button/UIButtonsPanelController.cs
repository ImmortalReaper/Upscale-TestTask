using System.Collections.Generic;
using Core.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace Feature.UIModule.Scripts.UIElements.Panel_Button
{
    public class UIButtonsPanelCobtroller : MonoBehaviour
    {
        [SerializeField] private List<UIPanelButton> panels = new();
        [SerializeField] private int defaultSelectedIndex = 0;
    
        private IInputService _inputService;
        private List<UnityAction> _buttonActions = new();
        private int _currentIndex = -1;

        [Inject]
        public void InjectDependencies(IInputService inputService)
        {
            _inputService = inputService;
        }
    
        private void Start()
        {
            SubscribeToButtons();
            if (panels.Count > 0)
                OnButtonClicked(defaultSelectedIndex);
        }

        private void OnEnable()
        {
            SelectButton(0);
            _inputService.UIInputService.OnNext += Next;
            _inputService.UIInputService.OnPrevious += Previous;
        }

        private void OnDisable()
        {
            _inputService.UIInputService.OnNext -= Next;
            _inputService.UIInputService.OnPrevious -= Previous;
        }

        private void OnDestroy()
        {
            UnsubscribeFromButtons();
        }

        private void SubscribeToButtons()
        {
            UnsubscribeFromButtons();
        
            for (int i = 0; i < panels.Count; i++)
            {
                int idx = i;
                UnityAction action = () => OnButtonClicked(idx);
            
                panels[i].UIButton.Button.onClick.AddListener(action);
                _buttonActions.Add(action);
            }
        }

        private void UnsubscribeFromButtons()
        {
            for (int i = 0; i < panels.Count && i < _buttonActions.Count; i++)
            {
                if (panels[i] != null && panels[i].UIButton != null && panels[i].UIButton.Button != null)
                {
                    panels[i].UIButton.Button.onClick.RemoveListener(_buttonActions[i]);
                }
            }
            _buttonActions.Clear();
        }
    
        private void OnButtonClicked(int index) => SelectButton(index);
        private void Next() => SelectButton(_currentIndex + 1);
        private void Previous() => SelectButton(_currentIndex - 1);
    
        private void SelectButton(int index)
        {
            if (index < 0 || index >= panels.Count || index == _currentIndex)
                return;

            _currentIndex = index;
            Selectable selectedObject = panels[_currentIndex].UIPanel.GetComponentInChildren<Selectable>();
            EventSystem.current.SetSelectedGameObject(selectedObject.gameObject);
            RefreshAll();
        }
    
        private void RefreshAll()
        {
            bool isActive;
            for (int i = 0; i < panels.Count; i++)
            {
                isActive = (i == _currentIndex);
            
                if (panels[i].UIButton.IsSelected && !isActive)
                    panels[i].UIButton.DeselectButton();
            
                if(isActive)
                    panels[i].UIButton.SelectButton();
            
                SetCanvasGroup(panels[i].UIPanel, isActive);
            
            }
        }

        private void SetCanvasGroup(CanvasGroup group, bool show)
        {
            group.alpha = show ? 1f : 0f;
            group.interactable = show;
            group.blocksRaycasts = show;
        }
    }
}
