using FMODUnity;
using UnityEngine;

namespace Core.Audio.Scripts
{
    [CreateAssetMenu(fileName = "MusicConfig", menuName = "Audio/MusicConfig")]
    public class MusicConfig : ScriptableObject
    {
        public EventReference backgroundMusic;
    }
}
