using System;

namespace Feature.UIModule.Scripts
{
    public interface IUIService {
        public event Action<BaseUIWindow, UIConfig> OnWindowShown;
        public event Action<BaseUIWindow, UIConfig> OnWindowHidden;
        public T Show<T>() where T : BaseUIWindow;
        public void Hide<T>() where T : BaseUIWindow;
        public bool IsVisible<T>() where T : BaseUIWindow;
        public T Load<T>() where T : BaseUIWindow;
        public bool TryGet<T>(out T window) where T : BaseUIWindow;
        public void Unload<T>() where T : BaseUIWindow;
        public UIConfig GetConfig<T>() where T : BaseUIWindow;
    }
}
