using System;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api;
using Modules.Common.WindowFsm.Runtime.Abstract;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class GameLoopPresenter : BaseWindowPresenter<GameLoopWindow>
    {
        private readonly IGameLoopView _view;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGameLoopService _gameLoopService;
        private readonly PlayerFactory _playerFactory;
        private readonly ILeveCompetitionService _levelCompetitionService;
        private readonly IAudioPlayerService _audioPlayerService;

        public GameLoopPresenter(IWindowFsm windowFsm,
            IGameLoopView view,
            IGameStateMachine gameStateMachine,
            IGameLoopService gameLoopService,
            PlayerFactory playerFactory,
            ILeveCompetitionService levelCompetitionService,
            IAudioPlayerService audioPlayerService)
            : base(windowFsm, view.Canvas)
        {
            _view = view;
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
            _levelCompetitionService = levelCompetitionService ?? throw new ArgumentNullException(nameof(levelCompetitionService));
            _audioPlayerService = audioPlayerService ?? throw new ArgumentNullException(nameof(audioPlayerService));
        }

        protected override void OnAfterEnable()
        {
            _view.CloseButton.Initialize();
            _view.CloseButton.Clicked += GoToMainMenu;

            _playerFactory.Create(_gameLoopService);
            _levelCompetitionService.StartCompetition();
            _audioPlayerService.PlayAmbient();
        }

        protected override void OnAfterDisable()
        {
            _view.CloseButton.Clicked -= GoToMainMenu;

            _levelCompetitionService.Stop();
            _audioPlayerService.StopAmbient();
        }

        private void GoToMainMenu()
        {
            _gameStateMachine.GoToMainMenu();
        }
    }
}