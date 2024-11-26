using Sirenix.OdinInspector;
using UnityEngine;

namespace Controller
{
    public class AudioController : MonoBehaviour
    {
        [FoldoutGroup("Sources")] [SerializeField]
        private AudioSource soundEffectAudioSource;

        [FoldoutGroup("Sources")] [SerializeField]
        private AudioSource musicAudioSource;

        [FoldoutGroup("Sources")] [SerializeField]
        private AudioSource playerEffectAudioSource;

        [FoldoutGroup("Effects")] [FoldoutGroup("Effects/Game")] [SerializeField]
        private AudioClip nextLevelClip;

        [FoldoutGroup("Effects")] [FoldoutGroup("Effects/Game")] [SerializeField]
        private AudioClip gainLifeClip;

        [FoldoutGroup("Effects")] [FoldoutGroup("Effects/Game")] [SerializeField]
        private AudioClip loseLifeClip;

        //Audio Clip - When the ball hits a brick
        [FoldoutGroup("Effects")] [FoldoutGroup("Effects/Player")] [SerializeField]
        private AudioClip onHitBrickAudio;

        //Audio Clip - When the ball hits a 'Kill zone'
        [FoldoutGroup("Effects")] [FoldoutGroup("Effects/Player")] [SerializeField]
        private AudioClip onHitKillAudio;

        //Audio Clip - When the ball hits a Wall/Paddle
        [FoldoutGroup("Effects")] [FoldoutGroup("Effects/Player")] [SerializeField]
        private AudioClip onHitWallAudio;

        /// <summary>
        /// Plays a sound effect from the appropriate audio source
        /// </summary>
        /// <param name="sfx">The sound effect to play (represented by enum)</param>
        public void PlaySoundEffect(BreakoutEnums.SoundEffect sfx)
        {
            switch (sfx)
            {
                case BreakoutEnums.SoundEffect.LifeLost:
                    soundEffectAudioSource.PlayOneShot(loseLifeClip);
                    break;
                case BreakoutEnums.SoundEffect.LifeGained:
                    soundEffectAudioSource.PlayOneShot(gainLifeClip);
                    break;
                case BreakoutEnums.SoundEffect.NextLevel:
                    soundEffectAudioSource.PlayOneShot(nextLevelClip);
                    break;
                case BreakoutEnums.SoundEffect.WallHit:
                    playerEffectAudioSource.PlayOneShot(onHitWallAudio);
                    break;
                case BreakoutEnums.SoundEffect.BrickHit:
                    playerEffectAudioSource.PlayOneShot(onHitBrickAudio);
                    break;
            }
        }

        /// <summary>
        /// Plays a given music track.
        /// </summary>
        /// <param name="music"></param>
        public void PlayMusic(AudioClip music)
        {
            musicAudioSource.clip = music;
            musicAudioSource.Play();
        }
    }
}