using CodeBase.Domain;

namespace CodeBase.Infrastructure
{
    public class AbilityBuilder
    {
        private readonly PlayerBuilder _playerBuilder;

        public AbilityBuilder(PlayerBuilder playerBuilder) => _playerBuilder = playerBuilder;

        public void Build(HeroModel heroModel) =>
            _playerBuilder.Build(heroModel.CurrentSelectedHero.InitialAbilityConfig);
    }
}