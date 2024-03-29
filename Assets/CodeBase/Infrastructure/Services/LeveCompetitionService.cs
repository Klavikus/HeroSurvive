using System;
using System.Collections.Generic;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enemies;
using CodeBase.Domain.Models;

namespace CodeBase.Infrastructure.Services
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

        private List<Enemy> _enemies;

        public event Action<int> WaveCompleted;
        public event Action AllWavesCompleted;
        public event Action<Enemy> EnemyKilled;

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
            _enemies = new List<Enemy>();
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
            foreach (Enemy enemy in _enemies)
            {
                enemy.Died -= OnEnemyDied;
                enemy.OutOfViewTimeout -= OnEnemyOutOfViewTimeout;
            }
        }

        private void SpawnNextWave()
        {
            _enemies.Clear();
            _maxWaveId = _competitionConfig.WavesData[_currentStageId].PerStage;

            Enemy[] waveEnemies =
                _enemySpawnService.SpawnWave(_competitionConfig.WavesData[_currentStageId].EnemiesSpawnData);
            _enemies.AddRange(waveEnemies);
            _enemiesInWave = waveEnemies.Length;
            _killedEnemiesCount = 0;

            foreach (Enemy enemy in waveEnemies)
            {
                enemy.UpdateProgression((float) _currentStageId / _maxStageId);
                enemy.Died += OnEnemyDied;
                enemy.OutOfViewTimeout += OnEnemyOutOfViewTimeout;
            }
        }

        private void OnEnemyOutOfViewTimeout(Enemy enemy) => _enemySpawnService.MoveCloserToPlayer(enemy);

        private void OnEnemyDied(Enemy enemy)
        {
            _modelProvider.Get<LevelUpModel>().HandleRewardedKill(enemy);
            
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