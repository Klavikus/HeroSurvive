using System;
using FMODUnity;

namespace GameCore.Source.Domain.Abilities
{
    [Serializable]
    public class AudioData
    {
        public EventReference FMOD;
        public bool IsPlayable;
    }
}