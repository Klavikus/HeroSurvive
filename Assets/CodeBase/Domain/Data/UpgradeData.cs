using System;
using UnityEngine;

namespace CodeBase.Domain.Data
{
    [Serializable]
    public class UpgradeData
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite Icon { get; set; }
        [field: SerializeField] public UpgradesLevelData[] Upgrades { get; set; }
    }
}