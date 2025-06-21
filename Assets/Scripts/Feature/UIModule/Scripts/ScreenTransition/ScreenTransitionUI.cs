using Feature.AnimationModule.Scripts;
using UnityEngine;

namespace Feature.UIModule.Scripts.ScreenTransition
{
    public class ScreenTransitionUI : BaseUIWindow
    {
        [SerializeField] private DOTweenSequenceAnimator fadeInAnimation;
        [SerializeField] private DOTweenSequenceAnimator fadeOutAnimation;
    
        private bool _fadeInPlaying = false;
        private bool _fadeOutPlaying = false;

        public bool FadeInPlaying => _fadeInPlaying;
        public bool FadeOutPlaying => _fadeOutPlaying;
    
        private void OnEnable()
        {
            fadeInAnimation.OnSequenceComplete += FadeInSequenceComplete;
            fadeInAnimation.OnSequenceKill += FadeInSequenceComplete;
            fadeOutAnimation.OnSequenceComplete += FadeOutSequenceComplete;
            fadeOutAnimation.OnSequenceKill += FadeOutSequenceComplete;
        }

        private void OnDisable()
        {
            fadeInAnimation.OnSequenceComplete -= FadeInSequenceComplete;
            fadeInAnimation.OnSequenceKill -= FadeInSequenceComplete;
            fadeOutAnimation.OnSequenceComplete -= FadeOutSequenceComplete;
            fadeOutAnimation.OnSequenceKill -= FadeOutSequenceComplete;
        }

        private void FadeOutSequenceComplete() => _fadeOutPlaying = false;
        private void FadeInSequenceComplete() => _fadeInPlaying = false;
    
        public void PlayFadeIn()
        {
            fadeInAnimation.PlaySequence();
            _fadeInPlaying = true;
        }

        public void PlayFadeOut()
        {
            fadeOutAnimation.PlaySequence();
            _fadeOutPlaying = true;
        }
    }
}
