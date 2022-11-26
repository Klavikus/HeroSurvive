using CodeBase.Infrastructure.Factories;
using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.StateMachine
{
    public class AbilityBuilder
    {
        private readonly PlayerBuilder _playerBuilder;

        public AbilityBuilder(PlayerBuilder playerBuilder) => _playerBuilder = playerBuilder;

        public void Build(HeroModel heroModel) =>
            _playerBuilder.Build(heroModel.CurrentSelectedHero.InitialAbilityConfig);
    }
}