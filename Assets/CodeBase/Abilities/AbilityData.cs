using System;
using System.Collections.Generic;
using CodeBase.Stats;
using UnityEngine;

namespace CodeBase.Abilities
{
    public enum AbilityBaseProperty
    {
        Damage,
        Penetration,
        AttackDelay,
        SpawnCount,
        BurstCount,
    }

    public class AbilityData
    {
        private Dictionary<AbilityBaseProperty, float> _baseProperty;
        public event Action<AbilityData> Changed;
        private Dictionary<AbilityBaseProperty, float> CreateBaseProperties()
        {
            Dictionary<AbilityBaseProperty, float> result = new Dictionary<AbilityBaseProperty, float>();

            foreach (string name in Enum.GetNames(typeof(AbilityBaseProperty)))
            {
                Enum.TryParse(name, out AbilityBaseProperty property);
                result.Add(property, 0f);
            }

            result[AbilityBaseProperty.SpawnCount] = SpawnCount;
            result[AbilityBaseProperty.BurstCount] = BurstCount;
            
            return result;
        }

        public AbilityData(AbilityConfigSO abilityConfig)
        {
            AttackType = abilityConfig.AttackType;
            WhatIsEnemy = abilityConfig.WhatIsEnemy;
            Damage = abilityConfig.Damage;
            MaxAffectedEnemy = abilityConfig.MaxAffectedEnemy;
            Penetration = abilityConfig.Penetration;
            IsLimitedPenetration = abilityConfig._isLimitedPenetration;
            AttackDelay = abilityConfig.AttackDelay;
            BurstCount = abilityConfig.BurstCount;
            MaxBurstCount = abilityConfig.MaxBurstCount;
            BurstFireDelay = abilityConfig.BurstFireDelay;
            SpawnPosition = abilityConfig.SpawnPosition;
            SpawnCount = abilityConfig.SpawnCount;
            Radius = abilityConfig.Radius;
            Arc = abilityConfig.Arc;
            MoveType = abilityConfig.MoveType;
            Speed = abilityConfig.Speed;
            RotationStep = abilityConfig.RotationStep;
            StartTimePercent = abilityConfig.StartTimePercent;
            EndTimePercent = abilityConfig.EndTimePercent;
            StartRadiusCurve = abilityConfig.StartRadiusCurve;
            MainRadiusCurve = abilityConfig.MainRadiusCurve;
            EndRadiusCurve = abilityConfig.EndRadiusCurve;
            AlignWithRotation = abilityConfig.AlignWithRotation;
            Size = abilityConfig.Size;
            Duration = abilityConfig.Duration;
            Cooldown = abilityConfig.Cooldown;
            AbilityView = abilityConfig.AbilityView;
            IsSelfParent = abilityConfig.IsSelfParent;
            _baseProperty = CreateBaseProperties();
        }

        public AttackType AttackType { get; private set; }

        public ContactFilter2D WhatIsEnemy { get; private set; }
        public int Damage { get; private set; }
        public int MaxAffectedEnemy { get; private set; }
        public int Penetration { get; private set; }
        public bool IsLimitedPenetration { get; private set; }
        public float AttackDelay { get; private set; }
        public int BurstCount { get; private set; }
        public int MaxBurstCount { get; private set; }
        public float BurstFireDelay { get; private set; }

        public SpawnType SpawnPosition { get; private set; }

        public int SpawnCount { get; private set; }
        public float Radius { get; private set; }
        public float Arc { get; private set; }

        public MoveType MoveType { get; private set; }

        public float Speed { get; private set; }
        public float RotationStep { get; private set; }
        public float StartTimePercent { get; private set; }
        public float EndTimePercent { get; private set; }
        public AnimationCurve StartRadiusCurve { get; private set; }
        public AnimationCurve MainRadiusCurve { get; private set; }
        public AnimationCurve EndRadiusCurve { get; private set; }
        public bool AlignWithRotation { get; private set; }

        public float Size { get; private set; }

        public float Duration { get; private set; }
        public float Cooldown { get; private set; }
        public AbilityProjection AbilityView { get; private set; }
        public bool IsSelfParent { get; private set; }

        public void UseModifiers(IReadOnlyDictionary<PlayerStat, float> playerStats)
        {
            Debug.Log($"UseModifiers SpawnCount {SpawnCount}");
            Debug.Log($"UseModifiers BurstCount {BurstCount}");
            if (MaxBurstCount > 1)
            {
                BurstCount = (int)(_baseProperty[AbilityBaseProperty.BurstCount] +playerStats[PlayerStat.ProjectilesCountModifier]);

                if (BurstCount > MaxBurstCount)
                {
                    BurstCount = MaxBurstCount;
                }
            }
            else
            {
                SpawnCount =  (int)(_baseProperty[AbilityBaseProperty.SpawnCount] +playerStats[PlayerStat.ProjectilesCountModifier]);
            }

            Debug.Log($"UseModifiers SpawnCount {SpawnCount}");
            Debug.Log($"UseModifiers BurstCount {BurstCount}");
            Changed?.Invoke(this);
        }
    }
}