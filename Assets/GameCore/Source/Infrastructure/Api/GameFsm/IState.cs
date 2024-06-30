using Cysharp.Threading.Tasks;

namespace GameCore.Source.Infrastructure.Api.GameFsm
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }
}