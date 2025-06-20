namespace Feature.UIModule.Scripts
{
    public class UIConfig {
        public string Prefab;
        public int SortingOrder;
        public UIWindowType WindowType = UIWindowType.Normal;

        public UIConfig(string prefab, int sortingOrder, UIWindowType windowType) {
            Prefab = prefab;
            SortingOrder = sortingOrder;
            WindowType = windowType;
        }
    }
    
    public enum UIWindowType
    {
        Normal,
        Modal,
        Popup
    }
}
