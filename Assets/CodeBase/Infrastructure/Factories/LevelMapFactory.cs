using CodeBase.Configs;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class LevelMapFactory
    {
        private readonly GameLoopConfigSO _gameLoopConfig;

        public LevelMapFactory(ConfigurationProvider configurationProvider) => 
            _gameLoopConfig = configurationProvider.GameLoopConfig;

        public void Create() => GameObject.Instantiate(_gameLoopConfig.LevelMapPrefab);
    }
}