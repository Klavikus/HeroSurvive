namespace Modules.Infrastructure.Interfaces.GameFsm
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}