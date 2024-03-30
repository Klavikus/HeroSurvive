using System;
using System.Collections.Generic;
using GameCore.Source.Application.GameFSM.States;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Core;
using GameCore.Source.Infrastructure.Core.Services.DI;

namespace GameCore.Source.Application.GameFSM
{
    public class GameStateMachine : IGameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;

        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader)
        {
            ServiceContainer serviceContainer = new ServiceContainer();
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] = new BootstrapState(this, sceneLoader, serviceContainer),
                [typeof(MainMenuState)] = new MainMenuState(sceneLoader, serviceContainer),
                [typeof(GameLoopState)] = new GameLoopState(sceneLoader, serviceContainer),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Update() => 
            _activeState.Update();

        public void GoToGameLoop() => 
            Enter<GameLoopState>();

        public void GoToMainMenu() => 
            Enter<MainMenuState>();

        private TState ChangeState<TState>() where TState : class, IExitableState
        {
            _activeState?.Exit();

            TState state = GetState<TState>();
            _activeState = state;

            return state;
        }

        private TState GetState<TState>() where TState : class, IExitableState => 
            _states[typeof(TState)] as TState;
    }
}