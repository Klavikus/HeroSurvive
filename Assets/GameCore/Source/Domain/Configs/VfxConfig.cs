using System;
using UnityEngine;

namespace GameCore.Source.Domain.Configs
{
    [Serializable]
    public class VfxConfig
    {
        [field: SerializeField] public GameObject KillPrefab { get; private set; }
    }
}