using System;
using UnityEngine;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public class UpgradesLevelData
    {
        [field: SerializeField] public AdditionalHeroProperty[] AdditionalHeroProperties { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
    }
}