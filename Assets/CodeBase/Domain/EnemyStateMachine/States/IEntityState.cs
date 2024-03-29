using System;

namespace CodeBase.Domain.EnemyStateMachine.States
{
    public interface IEntityState : IDisposable
    {
        event Action<IEntityState> NeedChangeState;
        void Enter();
        void Exit();
        void Update();
    }
}