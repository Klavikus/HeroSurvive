using System;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Domain.Models;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class HeroPresenter : IPresenter
    {
        private readonly PlayerController _playerController;
        private readonly HeroModel _heroModel;
        private readonly IGameLoopService _gameLoopService;
        private readonly PlayerFactory _playerFactory;

        public HeroPresenter(
            PlayerController playerController,
            HeroModel heroModel,
            IGameLoopService gameLoopService)
        {
            _playerController = playerController;
            _heroModel = heroModel;
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
        }

        public void Enable()
        {
            _playerFactory.Create(_gameLoopService);
        }

        public void Disable()
        {
        }
    }
}