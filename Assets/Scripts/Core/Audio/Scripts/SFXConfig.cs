using FMODUnity;
using UnityEngine;

namespace Core.Audio.Scripts
{
    [CreateAssetMenu(fileName = "SFXConfig", menuName = "Audio/SFXConfig")]
    public class SFXConfig : ScriptableObject
    {
        public EventReference HoverUI;
        public EventReference ClickUI;
    }
}
