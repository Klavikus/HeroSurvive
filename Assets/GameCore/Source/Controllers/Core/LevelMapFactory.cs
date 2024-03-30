using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Services;
using UnityEngine;

namespace GameCore.Source.Controllers.Core
{
    public class LevelMapFactory
    {
        private readonly GameLoopConfigSO _gameLoopConfig;

        public LevelMapFactory(IConfigurationProvider configurationProvider) => 
            _gameLoopConfig = configurationProvider.GameLoopConfig;

        public void Create() => GameObject.Instantiate(_gameLoopConfig.LevelMapPrefab);
    }
}