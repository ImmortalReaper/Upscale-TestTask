namespace Feature.UIModule.Scripts
{
    public interface IUIService {
        public T Show<T>() where T : BaseUIWindow;
        public void Hide<T>() where T : BaseUIWindow;
        public bool IsVisible<T>() where T : BaseUIWindow;
    }
}
