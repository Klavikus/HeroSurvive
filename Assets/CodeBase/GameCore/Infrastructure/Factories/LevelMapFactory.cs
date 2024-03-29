using CodeBase.GameCore.Configs;
using CodeBase.GameCore.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.GameCore.Infrastructure.Factories
{
    public class LevelMapFactory
    {
        private readonly GameLoopConfigSO _gameLoopConfig;

        public LevelMapFactory(IConfigurationProvider configurationProvider) => 
            _gameLoopConfig = configurationProvider.GameLoopConfig;

        public void Create() => GameObject.Instantiate(_gameLoopConfig.LevelMapPrefab);
    }
}