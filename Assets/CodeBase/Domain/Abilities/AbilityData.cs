using System;
using System.Collections.Generic;
using CodeBase.Configs;
using CodeBase.Domain.Enums;
using UnityEngine;

namespace CodeBase.Domain.Abilities
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
        private readonly Dictionary<BaseProperty, float> _upgradeModifiers = new Dictionary<BaseProperty, float>();
        private readonly Dictionary<BaseProperty, float> _heroUpgradeModifiers = new Dictionary<BaseProperty, float>();
        private readonly Dictionary<BaseProperty, float> _resultProperties = new Dictionary<BaseProperty, float>();

        public AbilityData(AbilityConfigSO abilityConfig)
        {
            AttackType = abilityConfig.AttackType;
            WhatIsEnemy = abilityConfig.WhatIsEnemy;
            BaseDamage = abilityConfig.Damage;
            Damage = BaseDamage;
            MaxAffectedEnemy = abilityConfig.MaxAffectedEnemy;
            Penetration = abilityConfig.Penetration;
            IsLimitedPenetration = abilityConfig._isLimitedPenetration;
            AttackDelay = abilityConfig.AttackDelay;
            BaseBurstCount = abilityConfig.BurstCount;
            BurstCount = BaseBurstCount;
            MaxBurstCount = abilityConfig.MaxBurstCount;
            BurstFireDelay = abilityConfig.BurstFireDelay;
            SpawnPosition = abilityConfig.SpawnPosition;
            BaseSpawnCount = abilityConfig.SpawnCount;
            SpawnCount = BaseSpawnCount;
            BaseRadius = abilityConfig.Radius;
            Radius = BaseRadius;
            Arc = abilityConfig.Arc;
            MoveType = abilityConfig.MoveType;
            BaseSpeed = abilityConfig.Speed;
            Speed = BaseSpeed;
            RotationStep = abilityConfig.RotationStep;
            StartTimePercent = abilityConfig.StartTimePercent;
            EndTimePercent = abilityConfig.EndTimePercent;
            StartRadiusCurve = abilityConfig.StartRadiusCurve;
            MainRadiusCurve = abilityConfig.MainRadiusCurve;
            EndRadiusCurve = abilityConfig.EndRadiusCurve;
            AlignWithRotation = abilityConfig.AlignWithRotation;
            FlipDirectionAllowed = abilityConfig.FlipDirectionAllowed;
            TargetingType = abilityConfig.TargetingType;
            BaseSize = abilityConfig.Size;
            Size = BaseSize;
            BaseDuration = abilityConfig.Duration;
            Duration = BaseDuration;
            BaseCooldown = abilityConfig.Cooldown;
            Cooldown = BaseCooldown;
            AbilityView = abilityConfig.AbilityView;
            IsSelfParent = abilityConfig.IsSelfParent;
        }

        public AttackType AttackType { get; private set; }

        public ContactFilter2D WhatIsEnemy { get; private set; }
        public int BaseDamage { get; private set; }
        public int Damage { get; private set; }
        public int MaxAffectedEnemy { get; private set; }
        public int Penetration { get; private set; }
        public bool IsLimitedPenetration { get; private set; }
        public float AttackDelay { get; private set; }
        public int BurstCount { get; private set; }
        public int BaseBurstCount { get; private set; }
        public int MaxBurstCount { get; private set; }
        public float BurstFireDelay { get; private set; }

        public SpawnType SpawnPosition { get; private set; }

        public int SpawnCount { get; private set; }
        public int BaseSpawnCount { get; private set; }
        public float Radius { get; private set; }
        public float BaseRadius { get; private set; }
        public float Arc { get; private set; }

        public MoveType MoveType { get; private set; }

        public float Speed { get; private set; }
        public float BaseSpeed { get; private set; }
        public float RotationStep { get; private set; }
        public float StartTimePercent { get; private set; }
        public float EndTimePercent { get; private set; }
        public AnimationCurve StartRadiusCurve { get; private set; }
        public AnimationCurve MainRadiusCurve { get; private set; }
        public AnimationCurve EndRadiusCurve { get; private set; }
        public bool AlignWithRotation { get; private set; }
        public bool FlipDirectionAllowed { get; private set; }
        public TargetingType TargetingType { get; private set; }

        public float BaseSize { get; private set; }
        public float Size { get; private set; }

        public float Duration { get; private set; }
        public float BaseDuration { get; private set; }
        public float Cooldown { get; private set; }
        public float BaseCooldown { get; private set; }
        public AbilityProjection AbilityView { get; private set; }
        public bool IsSelfParent { get; private set; }

        public void UpdateUpgradeModifiers(IReadOnlyList<AbilityUpgradeData> abilityUpgradesData)
        {
            _upgradeModifiers.Clear();

            foreach (AbilityUpgradeData abilityUpgradeData in abilityUpgradesData)
            {
                if (_upgradeModifiers.ContainsKey(abilityUpgradeData.PropertyType))
                    _upgradeModifiers[abilityUpgradeData.PropertyType] += abilityUpgradeData.Value;
                else
                    _upgradeModifiers.Add(abilityUpgradeData.PropertyType, abilityUpgradeData.Value);
            }

            CalculateResultModifiers();
            UseModifiers();
        }

        public void UpdateHeroModifiers(IReadOnlyDictionary<BaseProperty, float> properties)
        {
            _heroUpgradeModifiers.Clear();

            foreach (KeyValuePair<BaseProperty, float> pair in properties)
                _heroUpgradeModifiers.Add(pair.Key, pair.Value);

            CalculateResultModifiers();
            UseModifiers();
        }

        private void CalculateResultModifiers()
        {
            _resultProperties.Clear();

            foreach (KeyValuePair<BaseProperty, float> keyValuePair in _heroUpgradeModifiers)
                if (_resultProperties.ContainsKey(keyValuePair.Key))
                    _resultProperties[keyValuePair.Key] += keyValuePair.Value;
                else
                    _resultProperties.Add(keyValuePair.Key, keyValuePair.Value);

            foreach (KeyValuePair<BaseProperty, float> keyValuePair in _upgradeModifiers)
                if (_resultProperties.ContainsKey(keyValuePair.Key))
                    _resultProperties[keyValuePair.Key] += keyValuePair.Value;
                else
                    _resultProperties.Add(keyValuePair.Key, keyValuePair.Value);
        }

        private void UseModifiers()
        {
            //Amount
            if (MaxBurstCount > 1)
            {
                BurstCount = (int) (BaseBurstCount + _resultProperties[BaseProperty.Amount]);

                if (BurstCount > MaxBurstCount)
                    BurstCount = MaxBurstCount;
            }
            else
            {
                SpawnCount = (int) (BaseSpawnCount + _resultProperties[BaseProperty.Amount]);
            }

            //Damage
            Damage = (int) (BaseDamage * (1 + _resultProperties[BaseProperty.Damage] / 100));

            //Cooldown
            Cooldown = BaseCooldown * (1 + _resultProperties[BaseProperty.Cooldown] / 100);

            //Duration
            Duration = BaseDuration * (1 + _resultProperties[BaseProperty.Duration] / 100);

            //Area
            Size = BaseSize * (1 + _resultProperties[BaseProperty.Area] / 100);
            Radius = BaseRadius * (1 + _resultProperties[BaseProperty.Area] / 100);

            //ProjectileSpeed
            Speed = BaseSpeed * (1 + _resultProperties[BaseProperty.ProjectileSpeed] / 100);

            // Changed?.Invoke(this);
        }
    }
}