using UnityEngine;

namespace Feature.ControllerPresets.Scripts
{
    [CreateAssetMenu(fileName = "Preset Manager", menuName = "UI/Controller/Preset Manager")]
    public class PresetManager : ScriptableObject 
    {
        public ControllerPreset KeyboardPreset;
        public ControllerPreset XboxPreset;
        public ControllerPreset DualSensePreset;
    }
}
