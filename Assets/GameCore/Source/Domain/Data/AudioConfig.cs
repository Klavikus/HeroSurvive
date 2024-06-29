using System;
using FMODUnity;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public class AudioConfig
    {
        public EventReference HitReference;
        public EventReference UpgradeBuyReference;
        public EventReference PlayerDiedReference;
        public EventReference StartLevelReference;
    }
}