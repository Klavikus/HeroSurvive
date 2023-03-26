using System;
using UnityEngine;

namespace CodeBase.Domain
{
    [Serializable]
    public class UpgradeData
    {
        [field: SerializeField] public string KeyName { get; set; }
        [field: SerializeField] public TranslatableString[] TranslatableNames { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public UpgradesLevelData[] Upgrades { get; set; }
    }
}