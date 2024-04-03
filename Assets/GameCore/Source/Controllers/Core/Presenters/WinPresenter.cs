using System;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Domain.Models;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.GamePauseSystem.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class WinPresenter : BaseWindowPresenter<WinWindow>
    {
        private readonly IWinView _view;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGameLoopService _gameLoopService;
        private readonly IGamePauseService _gamePauseService;
        private readonly ILeveCompetitionService _leveCompetitionService;
        private readonly CurrencyModel _currencyModel;

        private bool _isDoubleRewardAvailable;

        public WinPresenter(IWindowFsm windowFsm,
            IWinView view,
            IGameStateMachine gameStateMachine,
            IGameLoopService gameLoopService,
            IGamePauseService gamePauseService,
            ILeveCompetitionService leveCompetitionService,
            CurrencyModel currencyModel)
            : base(windowFsm, view.Canvas)
        {
            _view = view;
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _gamePauseService = gamePauseService ?? throw new ArgumentNullException(nameof(gamePauseService));
            _leveCompetitionService =
                leveCompetitionService ?? throw new ArgumentNullException(nameof(leveCompetitionService));
            _currencyModel = currencyModel ?? throw new ArgumentNullException(nameof(currencyModel));
        }

        protected override void OnAfterEnable()
        {
            _view.Initialize();
            _view.GoldCounter.Initialize(0);
            _view.KillCounter.Initialize(0);

            UpdateVisibilityDoubleRewardButton();

            _view.ContinueButton.Clicked += OnContinueButtonClicked;
            _view.DoubleRewardButton.Clicked += OnDoubleRewardButtonClicked;

            _leveCompetitionService.AllWavesCompleted += OnAllWavesCompleted;
        }

        protected override void OnAfterDisable()
        {
            _view.ContinueButton.Clicked -= OnContinueButtonClicked;
            _view.DoubleRewardButton.Clicked -= OnDoubleRewardButtonClicked;
            _leveCompetitionService.AllWavesCompleted -= OnAllWavesCompleted;
        }

        protected override void OnAfterOpened()
        {
            _view.GoldCounter.Count(_leveCompetitionService.TotalGoldGained);
            _view.KillCounter.Count(_leveCompetitionService.TotalKilledEnemiesCount);
        }

        private void OnContinueButtonClicked()
        {
            WindowFsm.Close<WinWindow>();
            _gamePauseService.InvokeByUI(false);
            _gameStateMachine.GoToMainMenu();
        }

        private void UpdateVisibilityDoubleRewardButton()
        {
            _view.DoubleRewardButton.gameObject.SetActive(_isDoubleRewardAvailable);
        }

        private void OnAllWavesCompleted()
        {
            _gamePauseService.InvokeByUI(true);
            WindowFsm.OpenWindow<WinWindow>();
        }

        private void OnDoubleRewardButtonClicked()
        {
            if (_isDoubleRewardAvailable)
                _isDoubleRewardAvailable = false;

            UpdateVisibilityDoubleRewardButton();
        }
    }
}