namespace Feature.UIModule.Scripts.NavigationHistoryService
{
    public interface IUINavigationHistoryService
    {
        public bool HasActiveModals { get; }
        public void Back();
        public void ClearBacktrace();
    }
}
