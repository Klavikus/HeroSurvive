using System.Collections;
using CodeBase.Infrastructure.StateMachine;
using UnityEngine;

namespace CodeBase.Infrastructure.States
{
    public class BootstrapState : IState
    {
        private const string InitialScene = "Initial";

        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter()
        {
            Application.targetFrameRate = Screen.currentResolution.refreshRate;
            RegisterServices();
            _sceneLoader.Load(InitialScene, onLoaded: EnterLoadLevel);
        }

        public void Exit()
        {
        }

        private void EnterLoadLevel() =>
            _stateMachine.Enter<LoadProgressState>();

        private void RegisterServices()
        {
            Debug.Log("RegisterServices");
            // _services.RegisterSingle<ISaveInfoNewer>(new SaveInfoNewer());
            // _services.Single<ISaveInfoNewer>().LoadData();
        }
    }
}