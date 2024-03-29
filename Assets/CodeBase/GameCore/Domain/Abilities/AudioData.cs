using System;
using FMODUnity;

namespace CodeBase.GameCore.Domain.Abilities
{
    [Serializable]
    public class AudioData
    {
        public EventReference FMOD;
        public bool IsPlayable;
    }
}