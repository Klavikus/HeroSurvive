using System;
using System.Collections.Generic;
using CodeBase.ForSort;

namespace CodeBase.Infrastructure.StateMachine
{
    public class GameStateMachine
    {
        private readonly Dictionary<Type, IExitableState> _states;

        private IExitableState _activeState;

        public GameStateMachine(SceneLoader sceneLoader, ConfigurationContainer configurationContainer,
            ICoroutineRunner coroutineRunner)
        {
            _states = new Dictionary<Type, IExitableState>
            {
                [typeof(BootstrapState)] =
                    new BootstrapState(this, sceneLoader, configurationContainer, coroutineRunner),
                [typeof(LoadProgressState)] = new LoadProgressState(this),
                [typeof(LoadLevelState)] = new LoadLevelState(this, sceneLoader),
                [typeof(GameLoopState)] = new GameLoopState(this, sceneLoader),
            };
        }

        public void Enter<TState>() where TState : class, IState
        {
            IState state = ChangeState<TState>();
            state.Enter();
        }

        public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
        {
            TState state = ChangeState<TState>();
            state.Enter(payload);
        }

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