using System;
using FMODUnity;

namespace CodeBase.Domain.Abilities
{
    [Serializable]
    public class AudioData
    {
        public EventReference FMOD;
        public bool IsPlayable;
    }
}