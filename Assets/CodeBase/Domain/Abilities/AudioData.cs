using System;
using FMODUnity;
using UnityEngine;

namespace CodeBase.Domain
{
    [Serializable]
    public class AudioData
    {
        public EventReference FMOD;
        public bool IsPlayable;
    }
}