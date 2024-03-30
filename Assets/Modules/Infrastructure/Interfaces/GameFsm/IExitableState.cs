namespace Modules.Infrastructure.Interfaces.GameFsm
{
    public interface IExitableState
    {
        void Exit();
        void Update();
    }
}