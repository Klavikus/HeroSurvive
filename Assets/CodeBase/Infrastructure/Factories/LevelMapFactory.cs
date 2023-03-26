using CodeBase.Configs;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class LevelMapFactory
    {
        private readonly GameLoopConfigSO _gameLoopConfig;

        public LevelMapFactory(IConfigurationProvider configurationProvider) => 
            _gameLoopConfig = configurationProvider.GameLoopConfig;

        public void Create() => GameObject.Instantiate(_gameLoopConfig.LevelMapPrefab);
    }
}