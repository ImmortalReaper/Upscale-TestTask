using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Core.Audio.Scripts
{
    public class AudioService : IAudioService
    {
        private bool _isSoundEventsDisabled = false;
        private EventInstance _backgroundMusic;
        private MusicConfig _musicConfig;
        private SFXConfig _sfxConfig;
        
        public MusicConfig MusicConfig => _musicConfig;
        public SFXConfig SFXConfig => _sfxConfig;
        
        public AudioService(MusicConfig musicConfig, SFXConfig sfxConfig)
        {
            _musicConfig = musicConfig;
            _sfxConfig = sfxConfig;
        }
        
        public void SetBackgroundMusic(EventReference stageOneMusic)
        {
            _backgroundMusic = CreateEventInstance(stageOneMusic);
            _backgroundMusic.start();
        }

        public void DisableSoundEvents() =>
            _isSoundEventsDisabled = true;
        
        public void EnableSoundEvents() =>
            _isSoundEventsDisabled = false;
        
        public void StopBackgroundMusic() =>
            _backgroundMusic.setPaused(true);
        
        public void PlayBackgroundMusic() =>
            _backgroundMusic.setPaused(false);
        
        public EventInstance CreateEventInstance(EventReference eventReference) =>
            RuntimeManager.CreateInstance(eventReference);
        
        public void PlayOneShot(EventReference sound, Vector3 position)
        {
            if(_isSoundEventsDisabled)
                return;
            
            RuntimeManager.PlayOneShot(sound, position);
        }
    }
}
