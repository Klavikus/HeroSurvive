using System;
using UnityEngine;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public struct DamageSourceData
    {
        [field: SerializeField] public ContactFilter2D WhatIsTarget { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int AttackRadius { get; private set; }
        [field: SerializeField] public int MaxAffectedTargets { get; private set; }

        public void UpdateProgression(float stageProgressionModifier) =>
            Damage = (int) (Damage * stageProgressionModifier);
    }
}