using System.Collections;
using CodeBase.ForSort;
using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private ConfigurationContainer _configurationContainer;

        private Game _game;

        private void Awake()
        {
            _game = new Game(this, _configurationContainer);
            _game.StateMachine.Enter<BootstrapState>();
            DontDestroyOnLoad(target: this);
        }

        public Coroutine Run(IEnumerator coroutine) => StartCoroutine(coroutine);
        public void Stop(Coroutine coroutine) => StopCoroutine(coroutine);
    }
}