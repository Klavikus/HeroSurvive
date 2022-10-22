using CodeBase.Abilities;

namespace CodeBase.Factories
{
    public class AbilityFactory
    {
        private readonly AbilityProjectionFactory _abilityProjectionFactory;
        private readonly CoroutineRunner _coroutineRunner;
        private readonly TargetService _targetService;

        public AbilityFactory(AbilityProjectionFactory abilityProjectionFactory, CoroutineRunner coroutineRunner,
            TargetService targetService)
        {
            _coroutineRunner = coroutineRunner;
            _targetService = targetService;
            _abilityProjectionFactory = abilityProjectionFactory;
        }

        public Ability Create(AbilityData abilityConfig, ProjectionPool projectionPool) =>
            new Ability(_abilityProjectionFactory, abilityConfig, _coroutineRunner, projectionPool,
                _targetService);
    }
}