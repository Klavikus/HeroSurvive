using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameCore.Source.Application.GameFSM.States;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Core.Services;
using GameCore.Source.Infrastructure.Core.Services.DI;

namespace GameCore.Source.Application.GameFSM
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;
        private readonly List<UniTask> _beforeExitHandlers;

        private IExitableState _activeState;
        private IExitableState _nextState;

        public GameStateMachine(SceneLoader sceneLoader)
        {
            ServiceContainer serviceContainer = new ServiceContainer();
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, serviceContainer),
                [typeof(LoadDataState)] = new LoadDataState(serviceContainer),
                [typeof(MainMenuState)] = new MainMenuState(sceneLoader, serviceContainer),
                [typeof(GameLoopState)] = new GameLoopState(sceneLoader, serviceContainer),
            };
        }

        public async UniTask Enter<TState>() where TState : class, IState
        {
            TState targetState = GetState<TState>();

            if (_activeState == targetState || _nextState == targetState)
                return;

            _nextState = targetState;

            await ChangeState(targetState);
        }

        public void Update() =>
            _activeState.Update();

        public async void GoToGameLoop() =>
            await Enter<GameLoopState>();

        public async void GoToMainMenu() =>
            await Enter<MainMenuState>();

        private async UniTask ChangeState(IState state)
        {
            if (_activeState != null)
                _activeState.Exit();

            _activeState = state;
            await state.Enter();
        }

        private TState GetState<TState>()
            where TState : class, IExitableState =>
            _states[typeof(TState)] as TState;
    }
}