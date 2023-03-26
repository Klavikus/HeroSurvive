using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Domain
{
    public class AudioPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip _backgroundMusic;
        [SerializeField] private AudioSource _mainAudioSource;
        [SerializeField] private AudioSource _vfxAudioSource;

        private Dictionary<AudioSourceType, AudioSource> _audioSources;

        private void Start()
        {
            _audioSources = new Dictionary<AudioSourceType, AudioSource>()
            {
                {AudioSourceType.Main, _mainAudioSource},
                {AudioSourceType.Secondary, _vfxAudioSource},
            };
        }

        public void PlayOneShot(AudioClip audioClip, AudioSourceType audioSourceType) =>
            _audioSources[audioSourceType].PlayOneShot(audioClip);

        public void Play(AudioClip audioClip, AudioSourceType audioSourceType)
        {
            _audioSources[audioSourceType].clip = audioClip;
            _audioSources[audioSourceType].Play();
        }
    }
}