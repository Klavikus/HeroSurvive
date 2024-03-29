using CodeBase.Domain.Models;
using CodeBase.Infrastructure.Factories;

namespace CodeBase.Infrastructure.Builders
{
    public class AbilityBuilder
    {
        private readonly PlayerBuilder _playerBuilder;

        public AbilityBuilder(PlayerBuilder playerBuilder) => _playerBuilder = playerBuilder;

        public void Build(HeroModel heroModel) =>
            _playerBuilder.Build(heroModel.CurrentSelectedHero.InitialAbilityConfig);
    }
}