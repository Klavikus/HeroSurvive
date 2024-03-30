using System;
using UnityEngine;

namespace GameCore.Source.Presentation.Api
{
    public interface IPoolableParticleSystem
    {
        event Action<IPoolableParticleSystem> Completed;
        GameObject GameObject { get; }
    }
}