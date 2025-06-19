using System;
using System.Collections.Generic;
using System.Linq;
using Core.AssetLoader;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Object = UnityEngine.Object;

namespace Feature.UIModule.Scripts
{
    public class UIService : IUIService, IDisposable {
        private DiContainer _container;
        private Dictionary<Type, UIConfig> _uiConfigs;
        private Dictionary<Type, BaseUIWindow> _windows = new();
        private Stack<BaseUIWindow> _windowBacktrace = new();
        private Stack<BaseUIWindow> _modalBacktrace = new();
        private Stack<BaseUIWindow> _closablePopupBacktrace = new();
        private IAddressablesAssetLoaderService _addressablesAssetLoaderService;
        
        private const int NORMAL_SORTING_BASE = 100;
        private const int MODAL_SORTING_BASE = 1000;
        private const int POPUP_SORTING_BASE = 2000;
        
        public bool HasActiveModals => _modalBacktrace.Any(w => w.IsVisible);
        public bool HasActiveClosablePopups => _closablePopupBacktrace.Any(w => w.IsVisible);

        public UIService(DiContainer container, Dictionary<Type, UIConfig> uiConfigs, IAddressablesAssetLoaderService addressablesAssetLoaderService) 
        {
            _container = container;
            _uiConfigs = uiConfigs;
            _addressablesAssetLoaderService = addressablesAssetLoaderService;
        }

        public T Show<T>() where T : BaseUIWindow 
        {
            Type type = typeof(T);
            
            try 
            {
                if (!_windows.TryGetValue(type, out var window)) 
                    window = CreateWindow<T>(type);
                
                if (window == null)
                    throw new InvalidOperationException($"Failed to create window of type {type.Name}");

                if (IsVisible<T>())
                    return (T)window;
                
                ShowWindow(window, type);
                return (T)window;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to show window {type.Name}: {ex.Message}", ex);
            }
        }

        public bool TryGet<T>(out T window) where T : BaseUIWindow 
        {
            Type type = typeof(T);
            if (_windows.TryGetValue(type, out var foundWindow) && foundWindow != null) 
            {
                window = (T)foundWindow;
                return true;
            }
            
            window = null;
            return false;
        }
        
        public T Load<T>() where T : BaseUIWindow 
        {
            Type type = typeof(T);
            
            if (!_windows.TryGetValue(type, out var window)) 
            {
                window = CreateWindow<T>(type);
            }
            
            if (window == null)
                throw new InvalidOperationException($"Failed to load window of type {type.Name}");
            
            window.Hide();
            return (T)window;
        }
        
        public void Unload<T>() where T : BaseUIWindow 
        {
            Type type = typeof(T);
            if (_windows.TryGetValue(type, out var window) && window != null) 
            {
                HideWindow(window);
                _windows.Remove(type);
                Object.Destroy(window.transform.root.gameObject);
            }
        }
        
        private T CreateWindow<T>(Type type) where T : BaseUIWindow
        {
            if (!_uiConfigs.TryGetValue(type, out var config)) 
                throw new ArgumentException($"UI Config for {type.Name} is not registered!");

            GameObject windowPrefab = _addressablesAssetLoaderService.LoadAsset<GameObject>(config.Prefab);
            if (windowPrefab == null)
                throw new InvalidOperationException($"Failed to load prefab {config.Prefab} for {type.Name}");

            var windowObject = _container.InstantiatePrefab(windowPrefab);
            var window = windowObject.GetComponentInChildren<T>(true);
            
            if (window == null)
                throw new InvalidOperationException($"Prefab {config.Prefab} doesn't contain component {type.Name}");

            _windows[type] = window;
            SetupWindow(window, config);
            
            return window;
        }

        private void SetupWindow(BaseUIWindow window, UIConfig config)
        {
            SetSortingOrder(window, config);
            
            window.OnShow += OnWindowShown;
            window.OnHide += OnWindowHidden;
        }

        private void ShowWindow(BaseUIWindow window, Type type)
        {
            var config = _uiConfigs[type];
            
            switch (config.WindowType)
            {
                case UIWindowType.Normal:
                    ShowNormalWindow(window);
                    break;
                case UIWindowType.Modal:
                    ShowModalWindow(window);
                    break;
                case UIWindowType.Popup:
                    ShowPopupWindow(window);
                    break;
                case UIWindowType.PopupWithClose:
                    ShowPopupWithCloseWindow(window);
                    break;
            }
            
            ShowAndSetFirstSelectable(window);
        }

        private void ShowNormalWindow(BaseUIWindow window)
        {
            if(!window.Backtraced) return;
            
            if (_windowBacktrace.Count > 0)
                _windowBacktrace.Peek().Hide();

            if (_windowBacktrace.Count == 0 || _windowBacktrace.Peek() != window)
                _windowBacktrace.Push(window);
        }

        private void ShowModalWindow(BaseUIWindow window)
        {
            if(!window.Backtraced) return;
            
            if (_modalBacktrace.Count > 0)
                _modalBacktrace.Peek().Hide();
            
            if (_modalBacktrace.Count == 0 || _modalBacktrace.Peek() != window)
                _modalBacktrace.Push(window);
        }
        
        private void ShowPopupWithCloseWindow(BaseUIWindow window)
        {
            if(!window.Backtraced) return;
            
            if (_closablePopupBacktrace.Count == 0 || _closablePopupBacktrace.Peek() != window)
                _closablePopupBacktrace.Push(window);
        }
        
        private void ShowPopupWindow(BaseUIWindow window)
        {
            
        }

        private void ShowAndSetFirstSelectable(BaseUIWindow window)
        {
            window.Show();
            EventSystem.current.SetSelectedGameObject(window.FirstSelectable);
        } 
        
        public void Back()
        {
            if (_closablePopupBacktrace.Count > 0)
            {
                var currentPopup = _closablePopupBacktrace.Pop();
                currentPopup.Hide();
                return;
            }
            
            if (_modalBacktrace.Count > 0)
            {
                var currentModal = _modalBacktrace.Pop();
                currentModal.Hide();
                
                if (_modalBacktrace.Count > 0)
                    _modalBacktrace.Peek().Show();
                return;
            }

            if (_windowBacktrace.Count > 1)
            {
                var current = _windowBacktrace.Pop();
                current.Hide();
                var previous = _windowBacktrace.Peek();
                ShowAndSetFirstSelectable(previous);
            }
        }
        
        public void ClearBacktrace() 
        {
            _windowBacktrace.Clear();
            _modalBacktrace.Clear();
            _closablePopupBacktrace.Clear();
        }
        
        public void Hide<T>() where T : BaseUIWindow 
        {
            Type type = typeof(T);
            if (_windows.TryGetValue(type, out var window) && window != null) 
                HideWindow(window);
        }

        private void HideWindow(BaseUIWindow window)
        {
            window.Hide();
            RemoveFromBacktraces(window);
        }

        private void RemoveFromBacktraces(BaseUIWindow window)
        {
            if (_windowBacktrace.Count > 0 && _windowBacktrace.Peek() == window)
                _windowBacktrace.Pop();
            
            if (_modalBacktrace.Count > 0 && _modalBacktrace.Peek() == window)
                _modalBacktrace.Pop();
            
            if (_closablePopupBacktrace.Count > 0 && _closablePopupBacktrace.Peek() == window)
                _closablePopupBacktrace.Pop();
        }

        public void HideAllModals()
        {
            while (_modalBacktrace.Count > 0)
            {
                var modal = _modalBacktrace.Pop();
                modal.Hide();
            }
        }

        public void HideAllPopups()
        {
            while (_closablePopupBacktrace.Count > 0)
            {
                var popup = _closablePopupBacktrace.Pop();
                popup.Hide();
            }
        }

        public bool IsVisible<T>() where T : BaseUIWindow
        {
            Type type = typeof(T);
        
            if (!_windows.TryGetValue(type, out var window) || window == null)
                return false;
        
            return window.IsVisible;
        }

        private void SetSortingOrder(BaseUIWindow window, UIConfig config) 
        {
            Canvas canvas = window.transform.root.GetComponent<Canvas>();
            if (canvas != null) 
            {
                canvas.overrideSorting = true;

                int baseSortingOrder = config.WindowType switch
                {
                    UIWindowType.Normal => NORMAL_SORTING_BASE,
                    UIWindowType.Modal => MODAL_SORTING_BASE,
                    UIWindowType.Popup => POPUP_SORTING_BASE,
                    UIWindowType.PopupWithClose => POPUP_SORTING_BASE,
                    _ => NORMAL_SORTING_BASE
                };
                
                canvas.sortingOrder = baseSortingOrder + config.SortingOrder;
            }
        }

        private void OnWindowShown(BaseUIWindow window)
        {
            
        }

        private void OnWindowHidden(BaseUIWindow window)
        {
            
        }
        
        public void CleanupInactiveWindows()
        {
            var inactiveWindows = _windows.Where(kvp => kvp.Value != null && !kvp.Value.IsVisible).ToList();
            
            foreach (var kvp in inactiveWindows)
            {
                if (kvp.Value != null)
                {
                    kvp.Value.OnShow -= OnWindowShown;
                    kvp.Value.OnHide -= OnWindowHidden;
                    
                    if (kvp.Value.gameObject != null)
                    {
                        Object.Destroy(kvp.Value.gameObject);
                    }
                }
                
                _windows.Remove(kvp.Key);
            }
        }
        
        public void Dispose()
        {
            foreach (var window in _windows.Values)
            {
                if (window != null)
                {
                    window.OnShow -= OnWindowShown;
                    window.OnHide -= OnWindowHidden;
                }
            }
            
            _windows.Clear();
            _windowBacktrace.Clear();
            _modalBacktrace.Clear();
            _closablePopupBacktrace.Clear();
        }
    }
}
