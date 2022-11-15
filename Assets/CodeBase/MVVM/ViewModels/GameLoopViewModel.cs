﻿using System;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enemies;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.ViewModels;

namespace CodeBase.Infrastructure.Factories
{
    public class GameLoopViewModel
    {
        private readonly GameLoopModel _gameLoopModel;
        private readonly LeveCompetitionService _leveCompetitionService;
        private readonly PlayerEventHandler _playerEventHandler;
        private readonly CurrencyViewModel _currencyViewModel;

        private int _currentEnemyKilled;
        private int _lastCompletedWave;
        private int _initialCurrency;
        private int _gainedCurrency;

        public event Action<int> WaveCompleted;
        public event Action<int> KilledChanged;
        public event Action AllWavesCompleted;
        public event Action PlayerDied;
        public event Action<int> RewardCurrencyChanged;

        public GameLoopViewModel(GameLoopModel gameLoopModel,
            LeveCompetitionService leveCompetitionService,
            PlayerEventHandler playerEventHandler, CurrencyViewModel currencyViewModel)
        {
            _gameLoopModel = gameLoopModel;
            _gameLoopModel.StartLevelInvoked += OnLevelStart;
            _leveCompetitionService = leveCompetitionService;
            _playerEventHandler = playerEventHandler;
            _currencyViewModel = currencyViewModel;
            _lastCompletedWave = 0;
            _playerEventHandler.Died += OnPlayerDied;
            _leveCompetitionService.WaveCompleted += OnWaveCompleted;
            _leveCompetitionService.EnemyKilled += OnEnemyKilled;
            _leveCompetitionService.AllWavesCompleted += OnAllWavesCompleted;

            _initialCurrency = _currencyViewModel.GetCurrentAmount();
            _gainedCurrency = 0;
            _currencyViewModel.CurrencyChanged += CurrencyChanged;
        }

        private void OnLevelStart(HeroData heroData)
        {
            _initialCurrency = _currencyViewModel.GetCurrentAmount();
            _gainedCurrency = 0;
            _currentEnemyKilled = 0;
            _lastCompletedWave = 0;
        }

        private void CurrencyChanged(int currentCurrency)
        {
            _gainedCurrency = currentCurrency - _initialCurrency;
            RewardCurrencyChanged?.Invoke(_gainedCurrency);
        }

        private void OnPlayerDied() => PlayerDied?.Invoke();

        private void OnAllWavesCompleted() => AllWavesCompleted?.Invoke();

        private void OnEnemyKilled(Enemy enemy)
        {
            _currentEnemyKilled++;
            KilledChanged?.Invoke(_currentEnemyKilled);
        }

        public void CloseLevel() => _gameLoopModel.InvokeLevelClose();

        public void StartLevel(HeroData heroData) => _gameLoopModel.InvokeStartLevel(heroData);

        public int GetAllWavesCount() => _leveCompetitionService.GetAllWavesCount();

        private void OnWaveCompleted(int completedWaveIndex)
        {
            _lastCompletedWave = completedWaveIndex + 1;
            WaveCompleted?.Invoke(completedWaveIndex + 1);
        }

        public int GetKillCount() => _currentEnemyKilled;

        public int GetClearedWaveCount() => _lastCompletedWave;
    }
}