using System;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Presenters.Base;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.GamePauseSystem.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters.GameLoop
{
    public class DeathPresenter : BaseWindowPresenter<DeathWindow>
    {
        private readonly IDeathView _view;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGameLoopService _gameLoopService;
        private readonly IGamePauseService _gamePauseService;

        private int _respawnsCount;

        public DeathPresenter(IWindowFsm windowFsm,
            IDeathView view,
            IGameStateMachine gameStateMachine,
            IGameLoopService gameLoopService,
            IGamePauseService gamePauseService)
            : base(windowFsm, view.Canvas)
        {
            _view = view;
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _gamePauseService = gamePauseService ?? throw new ArgumentNullException(nameof(gamePauseService));
        }

        protected override void OnAfterEnable()
        {
            _view.Initialize();

            _view.ResurrectButton.Clicked += _gameLoopService.ResurrectPlayer;
            _view.BackToMenuButton.Clicked += _gameStateMachine.GoToMainMenu;

            UpdateVisibilityResurrectButton();

            _view.DoubleRewardAdsButton.gameObject.SetActive(false);

            _gameLoopService.PlayerDied += OnPlayerDied;
            _gameLoopService.PlayerResurrected += OnPlayerResurrectInvoked;
        }

        protected override void OnAfterDisable()
        {
            _view.ResurrectButton.Clicked -= _gameLoopService.ResurrectPlayer;
            _view.BackToMenuButton.Clicked -= _gameStateMachine.GoToMainMenu;
            _gameLoopService.PlayerDied -= OnPlayerDied;
            _gameLoopService.PlayerResurrected -= OnPlayerResurrectInvoked;
        }

        private void OnPlayerDied()
        {
            _gamePauseService.InvokeByUI(true);
            WindowFsm.OpenWindow<DeathWindow>();
        }

        private void OnPlayerResurrectInvoked()
        {
            UpdateVisibilityResurrectButton();
            WindowFsm.Close<DeathWindow>();
            _gamePauseService.InvokeByUI(false);
        }

        private void UpdateVisibilityResurrectButton()
        {
            //TODO: Fix with gameLoop event
            if (_respawnsCount <= 1)
            {
                _respawnsCount++;
                _view.ResurrectButton.gameObject.SetActive(true);
            }
            else
            {
                _view.ResurrectButton.gameObject.SetActive(false);
            }
        }
    }
}