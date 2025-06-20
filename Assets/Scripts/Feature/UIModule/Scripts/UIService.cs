using System;
using System.Collections.Generic;
using System.Linq;
using Core.AssetLoader;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Feature.UIModule.Scripts
{
    public class UIService : IUIService, IDisposable {
        private DiContainer _container;
        private Dictionary<Type, UIConfig> _uiConfigs;
        private Dictionary<Type, BaseUIWindow> _windows = new();
        private IAddressablesAssetLoaderService _addressablesAssetLoaderService;
        
        private const int NORMAL_SORTING_BASE = 100;
        private const int MODAL_SORTING_BASE = 1000;
        private const int POPUP_SORTING_BASE = 2000;
        
        public event Action<BaseUIWindow, UIConfig> OnWindowShown;
        public event Action<BaseUIWindow, UIConfig> OnWindowHidden;

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
                
                OnWindowShown?.Invoke(window, _uiConfigs[type]);
                window.Show();
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
                window.Hide();
                OnWindowHidden?.Invoke(window, _uiConfigs[type]);
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
            SetSortingOrder(window, config);
            
            return window;
        }
        
        public void Hide<T>() where T : BaseUIWindow 
        {
            Type type = typeof(T);
            if (_windows.TryGetValue(type, out var window) && window != null)
            {
                window.Hide();
                OnWindowHidden?.Invoke(window, _uiConfigs[type]);
            }
        }

        public bool IsVisible<T>() where T : BaseUIWindow
        {
            Type type = typeof(T);
        
            if (!_windows.TryGetValue(type, out var window) || window == null)
                return false;
        
            return window.IsVisible;
        }
        
        public UIConfig GetConfig<T>() where T : BaseUIWindow 
        {
            Type type = typeof(T);
            if (_uiConfigs.TryGetValue(type, out var config))
                return config;
            
            throw new KeyNotFoundException($"UI Config for {type.Name} is not registered!");
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
                    _ => NORMAL_SORTING_BASE
                };
                
                canvas.sortingOrder = baseSortingOrder + config.SortingOrder;
            }
        }
        
        public void CleanupInactiveWindows()
        {
            var inactiveWindows = _windows.Where(kvp => kvp.Value != null && !kvp.Value.IsVisible).ToList();
            
            foreach (var kvp in inactiveWindows)
            {
                if (kvp.Value.gameObject != null)
                    Object.Destroy(kvp.Value.gameObject);
                
                _windows.Remove(kvp.Key);
            }
        }
        
        public void Dispose()
        {
            _windows.Clear();
        }
    }
}
