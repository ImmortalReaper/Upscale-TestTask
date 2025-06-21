using Core.Input;
using Feature.ControllerPresets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Feature.UIModule.Scripts.UIElements.HotKey
{
    public class UIHotkeyController : MonoBehaviour
    {
        [SerializeField] private ControllerPreset.HotKeyType hotKeyType;
        [SerializeField] private string hotKeyLabel;
        [SerializeField] private TextMeshProUGUI hotKeyLabelText;
        [Header("Icon")]
        [SerializeField] private Transform iconContent;
        [SerializeField] private Image icon;
        [Header("Text")]
        [SerializeField] private Transform textContent;
        [SerializeField] private TextMeshProUGUI text;
    
        private PresetManager _presetManager;
        private IInputDeviceService _inputDeviceService;
    
        [Inject]
        public void InjectDependencies(PresetManager presetManager, IInputDeviceService inputDeviceService)
        {
            _presetManager = presetManager;
            _inputDeviceService = inputDeviceService;
        }

        private void Start()
        {
            if (hotKeyLabelText != null)
                hotKeyLabelText.text = hotKeyLabel;
            HandleInputDeviceChanged(_inputDeviceService.CurrentDevice);
        }

        private void OnEnable()
        {
            _inputDeviceService.OnDeviceChanged += HandleInputDeviceChanged;
            HandleInputDeviceChanged(_inputDeviceService.CurrentDevice);
        }

        private void OnDisable()
        {
            _inputDeviceService.OnDeviceChanged -= HandleInputDeviceChanged;
        }

        private void HandleInputDeviceChanged(ActiveInputDevice activeInputDevice)
        {
            switch (activeInputDevice)
            {
                case ActiveInputDevice.KeyboardAndMouse:
                    SetPreset(_presetManager.KeyboardPreset);
                    break;
                case ActiveInputDevice.Xbox:
                    SetPreset(_presetManager.XboxPreset);
                    break;
                case ActiveInputDevice.DualSense:
                    SetPreset(_presetManager.DualSensePreset);
                    break;
                default:
                    SetPreset(_presetManager.KeyboardPreset);
                    break;
            }
        }
    
        private void SetPreset(ControllerPreset preset)
        {
            var item = preset.items.Find(x => x.hotKeyType == hotKeyType);
            if (item == null) return;

            if (item.itemType == ControllerPreset.ItemType.Icon)
            {
                iconContent.gameObject.SetActive(true);
                textContent.gameObject.SetActive(false);
                icon.sprite = item.itemIcon;
            }
            else
            {
                iconContent.gameObject.SetActive(false);
                textContent.gameObject.SetActive(true);
                text.text = item.itemText;
            }
        }
    }
}
