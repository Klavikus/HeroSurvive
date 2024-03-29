using System;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Abilities
{
    [Serializable]
    public struct AbilityModifiersMask
    {
        [field: SerializeField] public bool UseAmount { get; private set; }
        [field: SerializeField] public bool UseDamage { get; private set; }
        [field: SerializeField] public bool UseCooldown { get; private set; }
        [field: SerializeField] public bool UseDuration { get; private set; }
        [field: SerializeField] public bool UseArea { get; private set; }
        [field: SerializeField] public bool UseProjectileSpeed { get; private set; }
    }
}