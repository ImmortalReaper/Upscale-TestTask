using System;
using System.Collections.Generic;
using Core.AssetLoader;
using UnityEngine;
using Zenject;

namespace Feature.UIModule.Scripts
{
    public class UIService : IUIService {
        private DiContainer _container;
        private Dictionary<Type, UIConfig> _uiConfigs;
        private Dictionary<Type, BaseUIWindow> _windows = new Dictionary<Type, BaseUIWindow>();
        private IAddressablesAssetLoaderService _addressablesAssetLoaderService;
    
        public UIService(DiContainer container, Dictionary<Type, UIConfig> uiConfigs, IAddressablesAssetLoaderService addressablesAssetLoaderService) {
            _container = container;
            _uiConfigs = uiConfigs;
            _addressablesAssetLoaderService = addressablesAssetLoaderService;
        }

        public T Show<T>() where T : BaseUIWindow {
            Type type = typeof(T);

            if (!_windows.TryGetValue(type, out var window)) {
                if (!_uiConfigs.TryGetValue(type, out var config)) {
                    throw new Exception($"UI Prefab for {type.Name} is not registered!");
                }
            
                GameObject windowPrefab = _addressablesAssetLoaderService.LoadAsset<GameObject>(config.Prefab);
                window = _container.InstantiatePrefab(windowPrefab).GetComponent<T>();
                _windows[type] = window;
            
                SetSortingOrder(window, config.SortingOrder);
            }

            window.Show();
            return (T)window;
        }
    
        public void Hide<T>() where T : BaseUIWindow {
            Type type = typeof(T);
            if (_windows.TryGetValue(type, out var window)) {
                if(window == null)
                    return;
                window.Hide();
            }
        }

        public bool IsVisible<T>() where T : BaseUIWindow
        {
            Type type = typeof(T);
        
            if (!_windows.TryGetValue(type, out var window))
                return false;
        
            if (window == null)
                return false;
        
            return window.IsVisible;
        }

        private void SetSortingOrder(BaseUIWindow window, int sortingOrder) {
            Canvas canvas = window.GetComponent<Canvas>();
            if (canvas != null) {
                canvas.overrideSorting = true;
                canvas.sortingOrder = sortingOrder;
            }
        }
    }
}
