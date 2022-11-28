using System;
using UnityEngine;

[Serializable]
public class AbilityUpgradeViewData
{
    [field: SerializeField] public Sprite Icon { get; private set; }

    [field: SerializeField] public string Name { get; private set; }

    public AbilityUpgradeViewData(Sprite icon, string name)
    {
        Icon = icon;
        Name = name;
    }
}