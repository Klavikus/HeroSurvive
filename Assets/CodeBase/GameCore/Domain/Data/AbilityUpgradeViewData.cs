using System;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Data
{
    [Serializable]
    public class AbilityUpgradeViewData
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public TranslatableString[] TranslatableName { get; private set; }
    }
}