﻿using System;
using GameCore.Source.Controllers.Api.Factories;
using GameCore.Source.Controllers.Api.Handlers;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Domain.Models;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api.GameLoop;
using JetBrains.Annotations;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.GamePauseSystem.Runtime;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters.GameLoop
{
    public class GameLoopPresenter : IPresenter
    {
        private readonly IWindowFsm _windowFsm;
        private readonly IGameLoopView _view;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGameLoopService _gameLoopService;
        private readonly PlayerFactory _playerFactory;
        private readonly ILeveCompetitionService _levelCompetitionService;
        private readonly IAudioPlayerService _audioPlayerService;
        private readonly IHealthViewBuilder _healthViewBuilder;
        private readonly CurrencyModel _currencyModel;
        private readonly LevelUpModel _levelUpModel;
        private readonly IGamePauseService _gamePauseService;
        private readonly IApplicationFocusChangeHandler _focusChangeHandler;
        private readonly ILocalizationService _localizationService;

        public GameLoopPresenter(
            IWindowFsm windowFsm,
            IGameLoopView view,
            IGameStateMachine gameStateMachine,
            IGameLoopService gameLoopService,
            PlayerFactory playerFactory,
            ILeveCompetitionService levelCompetitionService,
            IAudioPlayerService audioPlayerService,
            IHealthViewBuilder healthViewBuilder,
            CurrencyModel currencyModel,
            LevelUpModel levelUpModel,
            IGamePauseService gamePauseService,
            IApplicationFocusChangeHandler focusChangeHandler)
        {
            _windowFsm = windowFsm;
            _view = view;
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
            _gameLoopService = gameLoopService ?? throw new ArgumentNullException(nameof(gameLoopService));
            _playerFactory = playerFactory ?? throw new ArgumentNullException(nameof(playerFactory));
            _levelCompetitionService = levelCompetitionService ??
                                       throw new ArgumentNullException(nameof(levelCompetitionService));
            _audioPlayerService = audioPlayerService ?? throw new ArgumentNullException(nameof(audioPlayerService));
            _healthViewBuilder = healthViewBuilder ?? throw new ArgumentNullException(nameof(healthViewBuilder));
            _currencyModel = currencyModel ?? throw new ArgumentNullException(nameof(currencyModel));
            _levelUpModel = levelUpModel ?? throw new ArgumentNullException(nameof(levelUpModel));
            _gamePauseService = gamePauseService ?? throw new ArgumentNullException(nameof(gamePauseService));
            _focusChangeHandler = focusChangeHandler ?? throw new ArgumentNullException(nameof(focusChangeHandler));
        }

        public void Enable()
        {
            _view.CloseButton.Initialize();
            _view.CloseButton.Clicked += OpenPauseMenu;

            UpdateCurrencyCounter(_currencyModel.CurrentAmount);
            UpdateEnemyCounter();
            UpdateLevelCounter(_levelUpModel.CurrentLevel);
            FillCompletionProgressBar(_levelUpModel.CurrentCompletionProgress);
            UpdateWaveCounter(_levelCompetitionService.CurrentWaveId);

            _currencyModel.CurrencyChanged += UpdateCurrencyCounter;
            _levelCompetitionService.EnemyKilled += UpdateEnemyCounter;
            _levelCompetitionService.WaveCompleted += UpdateWaveCounter;
            _levelUpModel.LevelChanged += UpdateLevelCounter;
            _levelUpModel.LevelProgressChanged += FillCompletionProgressBar;

            _playerFactory.Create(_gameLoopService);
            _gameLoopService.NotifyPlayerRespawn();
            _levelCompetitionService.StartCompetition();
            _audioPlayerService.PlayAmbient();

            _gamePauseService.Paused += OnPaused;
            _focusChangeHandler.FocusDropped += _gamePauseService.InvokeByFocusChanging;
        }

        public void Disable()
        {
            _view.CloseButton.Clicked -= OpenPauseMenu;

            _currencyModel.CurrencyChanged -= UpdateCurrencyCounter;
            _levelCompetitionService.EnemyKilled -= UpdateEnemyCounter;
            _levelUpModel.LevelChanged -= UpdateLevelCounter;
            _levelUpModel.LevelProgressChanged -= FillCompletionProgressBar;

            _levelCompetitionService.Stop();
            _audioPlayerService.StopAmbient();

            _gamePauseService.Paused -= OnPaused;
            _focusChangeHandler.FocusDropped -= _gamePauseService.InvokeByFocusChanging;
        }

        private void OpenPauseMenu() =>
            _windowFsm.OpenWindow<PauseWindow>();

        private void UpdateCurrencyCounter(int _) =>
            _view.GoldCountText.text = _levelCompetitionService.TotalGoldGained.ToString();

        private void UpdateEnemyCounter() =>
            _view.KillCountText.text = _levelCompetitionService.TotalKilledEnemiesCount.ToString();

        private void UpdateLevelCounter(int level) =>
            _view.LevelCountText.text = level.ToString();

        private void FillCompletionProgressBar(float completePercentage) =>
            _view.LevelCompletionImage.fillAmount = completePercentage;

        private void UpdateWaveCounter(int wave) =>
            _view.WaveCountText.text = (wave + 1).ToString();

        private void OnPaused()
        {
            if (_gamePauseService.IsInvokeByUI) return;

            _windowFsm.OpenWindow<PauseWindow>();
        }
    }
}