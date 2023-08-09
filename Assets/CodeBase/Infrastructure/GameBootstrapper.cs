using System.Collections;
using CodeBase.Domain;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
    {
        [SerializeField] private ConfigurationContainer _configurationContainer;
        [SerializeField] private AudioPlayer _audioPlayer;

        private Game _game;

        private void Awake()
        {
            _game = new Game(this, _configurationContainer, _audioPlayer);
            _game.StateMachine.Enter<BootstrapState>();
            DontDestroyOnLoad(target: this);
        }

        public Coroutine Run(IEnumerator coroutine) => StartCoroutine(coroutine);
        public void Stop(Coroutine coroutine) => StopCoroutine(coroutine);
    }
}