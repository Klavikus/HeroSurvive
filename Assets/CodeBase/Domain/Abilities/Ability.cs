using System.Collections;
using System.Collections.Generic;
using CodeBase.Domain.Enums;
using CodeBase.ForSort;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Pools;
using UnityEngine;

namespace CodeBase.Domain.Abilities
{
    public class Ability
    {
        private AbilityData _abilityData;
        private ICoroutineRunner _coroutineRunner;
        private ProjectionPool _projectionPool;
        private Transform _pivotObject;
        private bool _onCooldown;
        private AbilityProjectionBuilder _projectionBuilder;

        private bool _IsInitialized;

        public Ability(AbilityProjectionBuilder projectionBuilder, AbilityData abilityData,
            ICoroutineRunner coroutineRunner, ProjectionPool projectionPool)
        {
            _projectionBuilder = projectionBuilder;
            _abilityData = abilityData;
            _coroutineRunner = coroutineRunner;
            _projectionPool = projectionPool;
        }

        public void Initialize(Transform pivotObject)
        {
            _pivotObject = pivotObject;
            _IsInitialized = true;
        }

        public void Execute()
        {
            if (_onCooldown || !_IsInitialized)
                return;

            _coroutineRunner.Run(ActivateProjections());
            _coroutineRunner.Run(StartCooldown());
        }

        public void Update(IReadOnlyDictionary<BaseProperty, float> stats) => _abilityData.UpdateHeroModifiers(stats);

        private IEnumerator ActivateProjections()
        {
            for (int i = 0; i < _abilityData.BurstCount; i++)
            {
                _projectionBuilder.Build(_abilityData, _pivotObject, _projectionPool);

                yield return new WaitForSeconds(_abilityData.BurstFireDelay);
            }
        }

        private IEnumerator StartCooldown()
        {
            _onCooldown = true;
            yield return new WaitForSeconds(_abilityData.Cooldown);
            _onCooldown = false;
        }
    }
}