using Cysharp.Threading.Tasks;

namespace GameCore.Source.Infrastructure.Api.Services
{
    public interface IProgressService
    {
        UniTask Load();
        UniTask Save();
        UniTask SyncWithCloud();
        int GetGold();
        void SetGold(int amount);
    }
}