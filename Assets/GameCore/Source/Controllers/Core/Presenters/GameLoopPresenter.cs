using System;
using GameCore.Source.Controllers.Api.Services;
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

        public GameLoopPresenter(
            IWindowFsm windowFsm,
            IGameLoopView view,
            IGameStateMachine gameStateMachine,
            IGameLoopService gameLoopService)
            : base(windowFsm, view.Canvas)
        {
            _view = view;
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
        }

        protected override void OnAfterEnable()
        {
            // _gameLoopViewModel = gameLoopViewModel;
            // _levelUpView.Initialize(levelUpViewModel, upgradeDescriptionBuilder);
            // _killCounterView.Initialize();
            // _currencyCounterView.Initialize();
            // _gameLoopViewModel.KilledChanged += _killCounterView.OnCounterChanged;
            // _gameLoopViewModel.RewardCurrencyChanged += _currencyCounterView.OnCounterChanged;
            // _winView.Initialize(gameLoopViewModel);
            // _diedView.Initialize(gameLoopViewModel);
            // _closeLevelButton.onClick.AddListener(OnCloseLevelButtonClicked);
            // _gameLoopViewModel.WaveCompleted += OnWaveCompleted;
            // _wavesSlider.maxValue = _gameLoopViewModel.GetAllWavesCount();
            // //TODO: Change to TextBuilder
            // _wavesSlider.value = 0;
            // _wavesCounter.text = "1";
            
            _view.CloseButton.Initialize();
            _view.CloseButton.Clicked += _gameStateMachine.GoToMainMenu;
            
            _gameLoopService.Start();
        }

        protected override void OnAfterDisable()
        {
            _view.CloseButton.Clicked -= _gameStateMachine.GoToMainMenu;
        }
    }
}