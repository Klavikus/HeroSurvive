using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Models;

namespace GameCore.Source.Controllers.Core.Factories
{
    public class AbilityBuilder
    {
        private readonly PlayerBuilder _playerBuilder;

        public AbilityBuilder(PlayerBuilder playerBuilder) => _playerBuilder = playerBuilder;

        public void Build(HeroModel heroModel, IGameLoopService gameLoopService) =>
            _playerBuilder.Build(heroModel.CurrentSelectedHero.InitialAbilityConfig, gameLoopService);
    }
}