using System;
using UnityEngine;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public struct ProgressionData
    {
        [field: SerializeField] public AnimationCurve EnemyAI { get; private set; }
        [field: SerializeField] public AnimationCurve Damageable { get; private set; }
        [field: SerializeField] public AnimationCurve DamageSource { get; private set; }
        [field: SerializeField] public AnimationCurve Loot { get; private set; }
    }
}