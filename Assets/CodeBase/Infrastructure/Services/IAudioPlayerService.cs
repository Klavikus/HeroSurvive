using FMODUnity;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public interface IAudioPlayerService : IService
    {
        void PlayHit(Vector3 position);
    }
}