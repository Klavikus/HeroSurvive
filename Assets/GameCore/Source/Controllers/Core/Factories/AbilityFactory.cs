using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Factories;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Controllers.Core.Presenters.GameLoop;
using GameCore.Source.Domain;
using GameCore.Source.Domain.Abilities;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Infrastructure.Api;
using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Infrastructure.Core.Pools;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Factories
{
    public class AbilityFactory : IAbilityFactory
    {
        private readonly AbilityProjectionBuilder _abilityProjectionBuilder;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ProjectionPool _projectionPool;

        public AbilityFactory(
            AbilityProjectionBuilder abilityProjectionBuilder,
            ICoroutineRunner coroutineRunner)
        {
            _abilityProjectionBuilder = abilityProjectionBuilder;
            _coroutineRunner = coroutineRunner;
        }

        public IAbilityController Create(
            AbilityConfigSO initialAbilityConfig,
            Transform transform)
        {
            AbilityData abilityData = new(initialAbilityConfig);

            //TODO: Replace _abilityProjectionBuilder.GetOrCreateProjectionPool() to projectionPoolFactory.Create()
            return Create(
                abilityData,
                _abilityProjectionBuilder.GetOrCreateProjectionPool(abilityData),
                initialAbilityConfig.UpgradeData,
                transform);
        }

        private AbilityController Create(
            AbilityData abilityData,
            IProjectionPool projectionPool,
            AbilityUpgradeData[] upgradesData,
            Transform transform)
        {
            return new AbilityController(
                abilityData,
                upgradesData,
                _coroutineRunner,
                projectionPool,
                _abilityProjectionBuilder,
                transform);
        }
    }
}