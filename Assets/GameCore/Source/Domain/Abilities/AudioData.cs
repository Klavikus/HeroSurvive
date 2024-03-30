using System;
using Mono.Cecil;

namespace GameCore.Source.Domain.Abilities
{
    [Serializable]
    public class AudioData
    {
        public EventReference FMOD;
        public bool IsPlayable;
    }
}