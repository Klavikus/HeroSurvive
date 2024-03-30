using System;
using UnityEngine;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public class UpgradeData
    {
        [field: SerializeField] public string KeyName { get; private set; }
        [field: SerializeField] public TranslatableString[] TranslatableNames { get; private set; }
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public UpgradesLevelData[] Upgrades { get; private set; }
    }
}