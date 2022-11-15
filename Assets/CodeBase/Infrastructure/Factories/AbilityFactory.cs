using System.Collections.Generic;
using CodeBase.Configs;
using CodeBase.Domain.Abilities;
using CodeBase.ForSort;
using CodeBase.Infrastructure.Pools;
using CodeBase.Infrastructure.StateMachine;

namespace CodeBase.Infrastructure.Factories
{
    public class AbilityFactory
    {
        private readonly AbilityProjectionBuilder _abilityProjectionBuilder;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly AbilityUpgradesProvider _abilityUpgradesProvider;
        private readonly ProjectionPool _projectionPool;

        public AbilityFactory(AbilityProjectionBuilder abilityProjectionBuilder,
            ICoroutineRunner coroutineRunner,
            AbilityUpgradesProvider abilityUpgradesProvider)
        {
            _abilityProjectionBuilder = abilityProjectionBuilder;
            _coroutineRunner = coroutineRunner;
            _abilityUpgradesProvider = abilityUpgradesProvider;
        }

        public Ability Create(AbilityConfigSO initialAbilityConfig)
        {
            AbilityData abilityData = _abilityUpgradesProvider.ConfigsByAbilityData[initialAbilityConfig];
            //TODO: Replace _abilityProjectionBuilder.GetOrCreateProjectionPool() to projectionPoolFactory.Create()
            return Create(abilityData, _abilityProjectionBuilder.GetOrCreateProjectionPool(abilityData));
        }

        private Ability Create(AbilityData abilityConfig, ProjectionPool projectionPool) =>
            new Ability(_abilityProjectionBuilder, abilityConfig, _coroutineRunner, projectionPool);
    }
}