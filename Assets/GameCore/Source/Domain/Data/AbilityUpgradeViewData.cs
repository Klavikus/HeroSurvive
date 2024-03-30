using System;
using UnityEngine;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public class AbilityUpgradeViewData
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public TranslatableString[] TranslatableName { get; private set; }
    }
}