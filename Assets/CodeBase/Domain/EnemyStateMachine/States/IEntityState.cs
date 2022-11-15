using System;

namespace CodeBase.Domain.EnemyStateMachine.States
{
    public interface IEntityState
    {
        event Action<IEntityState> NeedChangeState;
        void Enter();
        void Exit();
        void Update();
    }
}