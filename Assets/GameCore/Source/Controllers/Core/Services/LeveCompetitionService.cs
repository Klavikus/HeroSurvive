using System;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;

namespace GameCore.Source.Controllers.Core.Services
{
    public class LeveCompetitionService : ILeveCompetitionService
    {
        private IEnemySpawnService _enemySpawnService;
        private readonly IModelProvider _modelProvider;
        private readonly IVfxService _vfxService;
        private readonly StageCompetitionConfigSO _competitionConfig;

        private int _currentWaveId;
        private int _maxWaveId;
        private int _allWavesCounter;

        private int _currentStageId;
        private int _maxStageId;

        private int _killedEnemiesCount;
        private int _enemiesInWave;

        private List<IEnemyController> _enemies;

        public event Action<int> WaveCompleted;
        public event Action AllWavesCompleted;
        public event Action<IEnemyController> EnemyKilled;

        public LeveCompetitionService
        (
            IEnemySpawnService enemySpawnService,
            IConfigurationProvider configurationProvider,
            IModelProvider modelProvider,
            IVfxService vfxService
        )
        {
            _enemySpawnService = enemySpawnService;
            _modelProvider = modelProvider;
            _vfxService = vfxService;
            _competitionConfig = configurationProvider.StageCompetitionConfig;
            _enemies = new List<IEnemyController>();
        }

        ~LeveCompetitionService()
        {
            UnsubscribeFromEnemies();
        }

        public void StartCompetition()
        {
            _currentWaveId = 0;
            _currentStageId = 0;
            _maxStageId = _competitionConfig.WavesData.Length;
            _allWavesCounter = 0;
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
            _killedEnemiesCount = 0;

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
            RewardData reward = new RewardData()
            {
                KillCurrency = enemy.KillCurrency,
                KillExperience = enemy.KillExperience
            };
            
            _modelProvider.Get<LevelUpModel>().HandleRewardedKill(reward);

            enemy.Died -= OnEnemyDied;
            enemy.OutOfViewTimeout -= OnEnemyOutOfViewTimeout;
            EnemyKilled?.Invoke(enemy);
            _killedEnemiesCount++;

            if (_killedEnemiesCount == _enemiesInWave)
            {
                _allWavesCounter++;
                _currentWaveId++;

                if (_currentWaveId == _maxWaveId)
                {
                    _currentStageId++;

                    if (_currentStageId == _maxStageId)
                    {
                        AllWavesCompleted?.Invoke();
                    }
                    else
                    {
                        _currentWaveId = 0;
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
}