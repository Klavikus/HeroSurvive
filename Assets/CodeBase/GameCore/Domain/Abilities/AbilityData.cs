using System.Collections.Generic;
using CodeBase.GameCore.Configs;
using CodeBase.GameCore.Domain.Abilities.Size;
using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Domain.Enums;
using CodeBase.Utilities.Extensions;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Abilities
{
    public class AbilityData
    {
        private readonly Dictionary<BaseProperty, float> _upgradeModifiers = new();
        private readonly Dictionary<BaseProperty, float> _heroUpgradeModifiers = new();
        private readonly Dictionary<BaseProperty, float> _resultProperties = new();
        private readonly AbilityModifiersMask _modifiersMask;

        private readonly int _baseDamage;
        private readonly int _baseBurstCount;
        private readonly int _maxBurstCount;
        private readonly int _baseSpawnCount;
        private readonly float _baseRadius;
        private readonly float _baseSpeed;
        private readonly float _baseSize;
        private readonly float _baseDuration;
        private readonly float _baseCooldown;
        
        private float _size;

        public AttackType AttackType { get; }
        public ContactFilter2D WhatIsEnemy { get; }
        public int MaxAffectedEnemy { get; }
        public int Penetration { get; }
        public bool IsLimitedPenetration { get; }
        public float AttackDelay { get; }
        public float BurstFireDelay { get; }
        public SpawnType SpawnPosition { get; }
        public float Arc { get; }
        public MoveType MoveType { get; }
        public float RotationStep { get; }
        public float StartTimePercent { get; }
        public float EndTimePercent { get; }
        public AnimationCurve StartRadiusCurve { get; }
        public AnimationCurve MainRadiusCurve { get; }
        public AnimationCurve EndRadiusCurve { get; }
        public bool AlignWithRotation { get; }
        public bool FlipDirectionAllowed { get; }
        public TargetingType TargetingType { get; }
        public AbilityProjection AbilityView { get; }
        public float Stagger { get; }
        public AudioData AudioData { get; }
        public SizeBehaviourData SizeBehaviourData { get; }
        public int Damage { get; private set; }
        public int BurstCount { get; private set; }
        public int SpawnCount { get; private set; }
        public float Radius { get; private set; }
        public float Speed { get; private set; }
        public float Duration { get; private set; }
        public float Cooldown { get; private set; }
        public float EnemyCheckRadius { get; }
        public int CheckCount { get; }
        public float AttackRadius { get; }

        public AbilityData(AbilityConfigSO abilityConfig)
        {
            SizeBehaviourData = abilityConfig.SizeBehaviourData;
            Stagger = abilityConfig.Stagger;
            _modifiersMask = abilityConfig.AbilityModifiersMask;
            AttackType = abilityConfig.AttackType;
            WhatIsEnemy = abilityConfig.WhatIsEnemy;
            _baseDamage = abilityConfig.Damage;
            Damage = _baseDamage;
            MaxAffectedEnemy = abilityConfig.MaxAffectedEnemy;
            Penetration = abilityConfig.Penetration;
            IsLimitedPenetration = abilityConfig.IsLimitedPenetration;
            AttackDelay = abilityConfig.AttackDelay;
            _baseBurstCount = abilityConfig.BurstCount;
            BurstCount = _baseBurstCount;
            _maxBurstCount = abilityConfig.MaxBurstCount;
            BurstFireDelay = abilityConfig.BurstFireDelay;
            SpawnPosition = abilityConfig.SpawnPosition;
            _baseSpawnCount = abilityConfig.SpawnCount;
            SpawnCount = _baseSpawnCount;
            _baseRadius = abilityConfig.Radius;
            Radius = _baseRadius;
            Arc = abilityConfig.Arc;
            MoveType = abilityConfig.MoveType;
            _baseSpeed = abilityConfig.Speed;
            Speed = _baseSpeed;
            RotationStep = abilityConfig.RotationStep;
            StartTimePercent = abilityConfig.StartTimePercent;
            EndTimePercent = abilityConfig.EndTimePercent;
            StartRadiusCurve = abilityConfig.StartRadiusCurve;
            MainRadiusCurve = abilityConfig.MainRadiusCurve;
            EndRadiusCurve = abilityConfig.EndRadiusCurve;
            AlignWithRotation = abilityConfig.AlignWithRotation;
            FlipDirectionAllowed = abilityConfig.FlipDirectionAllowed;
            TargetingType = abilityConfig.TargetingType;
            _baseSize = abilityConfig.Size;
            _size = _baseSize;
            _baseDuration = abilityConfig.Duration;
            Duration = _baseDuration;
            _baseCooldown = abilityConfig.Cooldown;
            Cooldown = _baseCooldown;
            AbilityView = abilityConfig.AbilityView;
            AudioData = abilityConfig.AudioData;
            EnemyCheckRadius = abilityConfig.EnemyCheckRadius;
            CheckCount = abilityConfig.CheckCount;
            AttackRadius = abilityConfig.AttackRadius;

            SizeBehaviourData.UpdateFullTime(_baseDuration);
            SizeBehaviourData.UpdateTargetSize(_baseSize);
        }

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
            if (_modifiersMask.UseAmount)
            {
                if (_maxBurstCount > 1)
                {
                    BurstCount = (int) (_baseBurstCount + _resultProperties[BaseProperty.Amount]);

                    if (BurstCount > _maxBurstCount)
                        BurstCount = _maxBurstCount;
                }
                else
                {
                    SpawnCount = (int) (_baseSpawnCount + _resultProperties[BaseProperty.Amount]);
                }
            }


            //Damage
            if (_modifiersMask.UseDamage)
                Damage = (int) (_baseDamage * (1 + _resultProperties[BaseProperty.Damage].AsPercentFactor()));

            //Cooldown
            if (_modifiersMask.UseCooldown)
                Cooldown = _baseCooldown * (1 + _resultProperties[BaseProperty.Cooldown].AsPercentFactor());

            //Duration
            if (_modifiersMask.UseDuration)
            {
                Duration = _baseDuration * (1 + _resultProperties[BaseProperty.Duration].AsPercentFactor());
                SizeBehaviourData.UpdateFullTime(Duration);
            }

            //Area
            if (_modifiersMask.UseArea)
            {
                _size = _baseSize * (1 + _resultProperties[BaseProperty.Area].AsPercentFactor());
                SizeBehaviourData.UpdateTargetSize(_size);
                Radius = _baseRadius * (1 + _resultProperties[BaseProperty.Area].AsPercentFactor());
            }

            //ProjectileSpeed
            if (_modifiersMask.UseProjectileSpeed)
                Speed = _baseSpeed * (1 + _resultProperties[BaseProperty.ProjectileSpeed].AsPercentFactor());
        }
    }
}