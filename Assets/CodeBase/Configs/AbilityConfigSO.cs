using CodeBase.Domain.Abilities;
using CodeBase.Domain.Abilities.Size;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enums;
using UnityEngine;

namespace CodeBase.Configs
{
    [CreateAssetMenu(menuName = "SO/CreateAbilityData/AbilityConfigSO", fileName = "AbilityConfigSO", order = 0)]
    public class AbilityConfigSO : ScriptableObject
    {
        [field: Header("HandleAttack Settings")]
        [field: SerializeField] public AttackType AttackType { get; private set; }
        [field: SerializeField] public ContactFilter2D WhatIsEnemy { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public float Stagger { get; private set; }
        [field: SerializeField] public int MaxAffectedEnemy { get; private set; }
        [field: SerializeField] public int Penetration { get; private set; }
        [field: SerializeField] public bool IsLimitedPenetration { get; private set; }
        [field: SerializeField] public float AttackDelay { get; private set; }
        [field: SerializeField] public int BurstCount { get; private set; }
        [field: SerializeField] public int MaxBurstCount { get; private set; }
        [field: SerializeField] public float BurstFireDelay { get; private set; }

        [field: Header("Spawn Position Settings")]
        [field: SerializeField] public SpawnType SpawnPosition { get;private set;  }
        [field: SerializeField] public int SpawnCount { get; private set; }
        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public float Arc { get; private set; }

        [field: Header("Movement Settings")] 
        [field: SerializeField] public MoveType MoveType { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float RotationStep { get; private set; }
        [field: SerializeField] public float StartTimePercent { get; private set; }
        [field: SerializeField] public float EndTimePercent { get; private set; }
        [field: SerializeField] public AnimationCurve StartRadiusCurve { get; private set; }
        [field: SerializeField] public AnimationCurve MainRadiusCurve { get; private set; }
        [field: SerializeField] public AnimationCurve EndRadiusCurve { get; private set; }
        [field: SerializeField] public bool AlignWithRotation { get; private set; }
        [field: SerializeField] public bool FlipDirectionAllowed { get; private set; }
        [field: SerializeField] public TargetingType TargetingType { get; private set; }

        [field: Header("Base Settings")]
        [field: SerializeField] public float Size { get; private set; }
        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public AbilityProjection AbilityView { get; private set; }
        [field: SerializeField] public bool IsSelfParent { get; private set; }

        [field: Header("Available upgrades")]
        [field: SerializeField] public AbilityUpgradeData[] UpgradeData { get; private set; }
        [field: SerializeField] public AbilityUpgradeViewData UpgradeViewData { get; private set; }
        [field: SerializeField] public AbilityModifiersMask AbilityModifiersMask { get; private set; }
        [field: SerializeField] public AudioData AudioData { get; private set; }
        [field: SerializeField] public SizeBehaviourData SizeBehaviourData { get; private set; }


        private void OnValidate()
        {
            if (BurstFireDelay * BurstCount >= Cooldown)
            {
                BurstFireDelay = Cooldown / BurstCount;
            }
        }
    }
}