using GameCore.Source.Infrastructure.Api.Services;
using GameCore.Source.Infrastructure.Core;
using GameCore.Source.Infrastructure.Core.Services;
using UnityEngine;

namespace GameCore.Source.Application.Factories
{
    internal class SceneLoaderFactory
    {
        private GameObject _gameObject;

        public SceneLoader Create()
        {
            _gameObject = new GameObject(nameof(SceneLoader));
            
            ICoroutineRunner coroutineRunner = _gameObject.AddComponent<CoroutineRunner>();
            
            return new SceneLoader(coroutineRunner);
        }
    }
}