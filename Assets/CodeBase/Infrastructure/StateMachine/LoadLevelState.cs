using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class LoadLevelState : IPayloadedState<string>
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly IMainMenuFactory _mainMenuFactory;
        
        public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _mainMenuFactory = AllServices.Container.Single<IMainMenuFactory>();
        }

        public void Enter(string sceneName) => _sceneLoader.Load(sceneName, onLoaded: OnLoaded);

        public void Exit() { }

        private void OnLoaded()
        {
            _mainMenuFactory.Initialization();
            _mainMenuFactory.ShowMenu();
        }
    }
}