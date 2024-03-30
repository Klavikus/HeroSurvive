namespace Source.Infrastructure.Api.GameFsm
{
    public interface IExitableState
    {
        void Exit();
        void Update();
    }
}