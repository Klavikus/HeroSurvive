using Source.Infrastructure.Api.Services;
using Source.Infrastructure.Core;
using UnityEngine;

namespace Source.Application.Factories
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