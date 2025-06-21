namespace Feature.UIModule.Scripts.BacktraceService
{
    public interface IUIBacktraceService
    {
        public bool HasActiveModals { get; }
        public void Back();
        public void ClearBacktrace();
    }
}
