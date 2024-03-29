using CodeBase.GameCore.Domain.Models;
using CodeBase.GameCore.Infrastructure.Factories;

namespace CodeBase.GameCore.Infrastructure.Builders
{
    public class AbilityBuilder
    {
        private readonly PlayerBuilder _playerBuilder;

        public AbilityBuilder(PlayerBuilder playerBuilder) => _playerBuilder = playerBuilder;

        public void Build(HeroModel heroModel) =>
            _playerBuilder.Build(heroModel.CurrentSelectedHero.InitialAbilityConfig);
    }
}