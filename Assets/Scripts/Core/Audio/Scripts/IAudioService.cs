using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Core.Audio.Scripts
{
    public interface IAudioService
    {
        public MusicConfig MusicConfig { get; }
        public SFXConfig SFXConfig { get; }
        public void SetBackgroundMusic(EventReference stageOneMusic);
        public void DisableSoundEvents();
        public void EnableSoundEvents();
        public void StopBackgroundMusic();
        public void PlayBackgroundMusic();
        public EventInstance CreateEventInstance(EventReference eventReference);
        public void PlayOneShot(EventReference sound, Vector3 position);
    }
}
