using UnityEngine;

namespace CodeBase.Abilities
{
    public enum AttackType
    {
        Continuous,
        Periodical,
        Single,
    }

    public enum MoveType
    {
        MoveUp,
        Orbital,
    }

    public enum SpawnType
    {
        Point,
        Circle,
        Arc
    }

    [CreateAssetMenu(menuName = "SO/CreateAbilityData/AbilityConfigSO", fileName = "AbilityConfigSO", order = 0)]
    public class AbilityConfigSO : ScriptableObject
    {
        [field: Header("Attack Settings")]
        [field: SerializeField]
        public AttackType AttackType { get; private set; }

        [field: SerializeField] public ContactFilter2D WhatIsEnemy { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int MaxAffectedEnemy { get; private set; }
        [field: SerializeField] public int Penetration { get; private set; }
        [field: SerializeField] public bool _isLimitedPenetration { get; private set; }
        [field: SerializeField] public float AttackDelay { get; private set; }
        [field: SerializeField] public int BurstCount { get; private set; }
        [field: SerializeField] public int MaxBurstCount { get; private set; }
        [field: SerializeField] public float BurstFireDelay { get; private set; }

        [field: Header("Spawn Position Settings")]
        [field: SerializeField]

        public SpawnType SpawnPosition { get; private set; }

        [field: SerializeField] public Vector2 Direction { get; private set; }
        [field: SerializeField] public int SpawnCount { get; private set; }
        [field: SerializeField] public float Radius { get; private set; }
        [field: SerializeField] public float Arc { get; private set; }

        [field: Header("Movement Settings")]
        [field: SerializeField]
        public MoveType MoveType { get; private set; }

        [field: SerializeField] public float Speed { get; private set; }
        [field: SerializeField] public float RotationStep { get; private set; }
        [field: SerializeField] public float StartTimePercent { get; private set; }
        [field: SerializeField] public float EndTimePercent { get; private set; }
        [field: SerializeField] public AnimationCurve StartRadiusCurve { get; private set; }
        [field: SerializeField] public AnimationCurve MainRadiusCurve { get; private set; }
        [field: SerializeField] public AnimationCurve EndRadiusCurve { get; private set; }
        [field: SerializeField] public bool AlignWithRotation { get; private set; }

        [field: Header("Base Settings")]
        [field: SerializeField]
        public float Size { get; private set; }

        [field: SerializeField] public float Duration { get; private set; }
        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public AbilityProjection AbilityView { get; private set; }
        [field: SerializeField] public bool IsSelfParent { get; private set; }

        private void OnValidate()
        {
            if (BurstFireDelay * BurstCount >= Cooldown)
            {
                Debug.LogWarning(
                    $"{nameof(BurstFireDelay)} * {nameof(BurstCount)} should be lower then {nameof(Cooldown)}");
                BurstFireDelay = Cooldown / BurstCount;
            }
        }
    }
}