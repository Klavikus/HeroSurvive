using System.Collections;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.States;
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

        public void Run(IEnumerator coroutine)
        {
            throw new System.NotImplementedException();
        }

        public T InstantiateGameObject<T>(T prefab, Vector2 position, Quaternion rotation, bool isSelfParent)
            where T : Object
        {
            throw new System.NotImplementedException();
        }
    }
}