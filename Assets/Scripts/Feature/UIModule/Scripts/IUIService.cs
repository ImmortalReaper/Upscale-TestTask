namespace Feature.UIModule.Scripts
{
    public interface IUIService {
        public bool HasActiveModals { get; }
        public bool HasActiveClosablePopups { get; }
        public T Show<T>() where T : BaseUIWindow;
        public void Hide<T>() where T : BaseUIWindow;
        public bool IsVisible<T>() where T : BaseUIWindow;
        public T Load<T>() where T : BaseUIWindow;
        public bool TryGet<T>(out T window) where T : BaseUIWindow;
        public void Unload<T>() where T : BaseUIWindow;
        public void Back();
        public void ClearBacktrace();
        public void HideAllModals();
        public void HideAllPopups();
        public void CleanupInactiveWindows();
    }
}
