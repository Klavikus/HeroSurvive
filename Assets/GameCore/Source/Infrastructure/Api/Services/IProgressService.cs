using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Data;

namespace GameCore.Source.Infrastructure.Api.Services
{
    public interface IProgressService
    {
        UniTask Load();
        UniTask Save();
        UniTask SyncWithCloud();
        int GetGold();
        void SetGold(int amount);
        T Get<T>(string id) where T : class, IEntity;
        void UpdateUpgradeData(string id, int level);
    }
}