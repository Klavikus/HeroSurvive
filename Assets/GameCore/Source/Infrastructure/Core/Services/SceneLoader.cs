using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using GameCore.Source.Infrastructure.Api.Services;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameCore.Source.Infrastructure.Core.Services
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner) =>
            _coroutineRunner = coroutineRunner;

        public void Load(string name, Action onLoaded = null) =>
            _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));

        public UniTask LoadAsync(string name) =>
            SceneManager.LoadSceneAsync(name).ToUniTask();

        private IEnumerator LoadScene(string nextScene, Action onLoaded = null)
        {
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(nextScene);

            while (!waitNextScene.isDone)
                yield return null;

            onLoaded?.Invoke();
        }
    }
}