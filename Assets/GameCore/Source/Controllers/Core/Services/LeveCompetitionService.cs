using System;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Providers;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;

namespace GameCore.Source.Controllers.Core.Services
{
    public class LeveCompetitionService : ILeveCompetitionService
    {
        private readonly IEnemySpawnService _enemySpawnService;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IModelProvider _modelProvider;
        private readonly IVfxService _vfxService;
        private readonly LevelUpModel _levelUpModel;
        private readonly StageCompetitionConfigSO _competitionConfig;
        private readonly List<IEnemyController> _enemies;

        private int _maxWaveId;
        private int _allWavesCounter;

        private int _currentStageId;
        private int _maxStageId;

        private int _enemiesInWave;
        private AccountModel _accountModel;

        public int KilledEnemiesCount { get; private set; }
        public int TotalKilledEnemiesCount { get; private set; }
        public int CurrentWaveId { get; private set; }
        public int TotalGoldGained { get; private set; }

        public event Action<int> WaveCompleted;
        public event Action AllWavesCompleted;
        public event Action EnemyKilled;

        public LeveCompetitionService
        (
            IEnemySpawnService enemySpawnService,
            IConfigurationProvider configurationProvider,
            IModelProvider modelProvider,
            IVfxService vfxService,
            LevelUpModel levelUpModel
        )
        {
            _enemySpawnService = enemySpawnService ?? throw new ArgumentNullException(nameof(enemySpawnService));
            _configurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _modelProvider = modelProvider ?? throw new ArgumentNullException(nameof(modelProvider));
            _vfxService = vfxService ?? throw new ArgumentNullException(nameof(vfxService));
            _levelUpModel = levelUpModel ?? throw new ArgumentNullException(nameof(levelUpModel));
            _competitionConfig = configurationProvider.StageCompetitionConfig;
            _enemies = new List<IEnemyController>();
        }

        ~LeveCompetitionService()
        {
            UnsubscribeFromEnemies();
        }

        public void StartCompetition()
        {
            TotalKilledEnemiesCount = 0;
            CurrentWaveId = 0;
            _currentStageId = 0;
            _maxStageId = _competitionConfig.WavesData.Length;
            _allWavesCounter = 0;
            _accountModel = _modelProvider.Get<AccountModel>();
            SpawnNextWave();
        }

        public void Stop()
        {
            //TODO: Fix spawn on die when game close in Editor
            _enemySpawnService.ClearEnemies();
            UnsubscribeFromEnemies();
            _vfxService.Clear();
        }

        public int GetAllWavesCount()
        {
            int resultCount = 0;
            foreach (StageData stageData in _competitionConfig.WavesData)
                resultCount += stageData.PerStage;

            return resultCount;
        }

        private void UnsubscribeFromEnemies()
        {
            foreach (IEnemyController enemy in _enemies)
            {
                enemy.Died -= OnEnemyDied;
                enemy.OutOfViewTimeout -= OnEnemyOutOfViewTimeout;
            }
        }

        private void SpawnNextWave()
        {
            _enemies.Clear();
            _maxWaveId = _competitionConfig.WavesData[_currentStageId].PerStage;

            IEnemyController[] waveEnemies =
                _enemySpawnService.SpawnWave(_competitionConfig.WavesData[_currentStageId].EnemiesSpawnData);
            _enemies.AddRange(waveEnemies);
            _enemiesInWave = waveEnemies.Length;
            KilledEnemiesCount = 0;

            foreach (IEnemyController enemy in waveEnemies)
            {
                enemy.UpdateProgression((float) _currentStageId / _maxStageId);
                enemy.Died += OnEnemyDied;
                enemy.OutOfViewTimeout += OnEnemyOutOfViewTimeout;
            }
        }

        private void OnEnemyOutOfViewTimeout(IEnemyController enemy)
        {
            // _enemySpawnService.MoveCloserToPlayer(enemy);
        }

        private void OnEnemyDied(IEnemyController enemy)
        {
            TotalKilledEnemiesCount++;

            RewardData reward = new()
            {
                KillCurrency = enemy.KillCurrency,
                KillExperience = enemy.KillExperience
            };

            TotalGoldGained += enemy.KillCurrency;

            _levelUpModel.HandleRewardedKill(reward);

            enemy.Died -= OnEnemyDied;
            enemy.OutOfViewTimeout -= OnEnemyOutOfViewTimeout;
            EnemyKilled?.Invoke();
            KilledEnemiesCount++;

            if (KilledEnemiesCount != _enemiesInWave)
                return;

            _allWavesCounter++;
            CurrentWaveId++;

            _accountModel.HandleWaveClearing();
            
            if (CurrentWaveId == _maxWaveId)
            {
                _currentStageId++;

                if (_currentStageId == _maxStageId)
                {
                    AllWavesCompleted?.Invoke();
                }
                else
                {
                    CurrentWaveId = 0;
                    WaveCompleted?.Invoke(_allWavesCounter);
                    SpawnNextWave();
                }
            }
            else
            {
                WaveCompleted?.Invoke(_allWavesCounter);
                SpawnNextWave();
            }
        }
    }
}