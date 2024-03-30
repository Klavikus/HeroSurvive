namespace GameCore.Source.Infrastructure.Api.GameFsm
{
    public interface IState : IExitableState
    {
        void Enter();
    }
}