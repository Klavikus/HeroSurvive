using FMODUnity;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class AudioPlayerService : IAudioPlayerService
    {
        private readonly IConfigurationProvider _configurationProvider;

        public AudioPlayerService(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public void PlayHit(Vector3 position) =>
            RuntimeManager.PlayOneShot(_configurationProvider.FMOD_HitReference, position);
    }
}