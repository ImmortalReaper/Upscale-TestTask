using System.Collections.Generic;
using UnityEngine;

namespace Feature.ControllerPresets.Scripts
{
    [CreateAssetMenu(fileName = "New Controller Preset", menuName = "UI/Controller/New Controller Preset")]
    public class ControllerPreset : ScriptableObject
    {
        public List<ControllerItem> items = new();

        public enum ItemType { Icon, Text }

        [System.Serializable]
        public class ControllerItem
        {
            public HotKeyType hotKeyType;
            public ItemType itemType;
            public Sprite itemIcon;
            public string itemText;
        }
    
        public enum HotKeyType
        {
            Exit,
            GoBack,
            Navigate,
            Select,
            Next,
            Previous,
            NextAlt,
            PreviousAlt
        }
    }
}