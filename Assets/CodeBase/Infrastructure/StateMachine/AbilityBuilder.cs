using CodeBase.Domain.Abilities;
using CodeBase.Infrastructure.Factories;
using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.StateMachine
{
    public class AbilityBuilder
    {
        private readonly PlayerBuilder _playerBuilder;
        private readonly AbilityFactory _abilityFactory;

        public AbilityBuilder(PlayerBuilder playerBuilder, AbilityFactory abilityFactory)
        {
            _playerBuilder = playerBuilder;
            _abilityFactory = abilityFactory;
        }

        public void Build(HeroModel heroModel)
        {
            Ability initialAbility = _abilityFactory.Create(heroModel.CurrentSelectedHero.InitialAbilityConfig);
            _playerBuilder.Build(initialAbility);
        }
    }
}