using Cysharp.Threading.Tasks;

namespace Modules.DAL.Abstract.Services
{
    public interface ILocalCloudContextService
    {
        UniTask LoadLocalContext();
        UniTask LoadCloudContext();
        UniTask SaveLocalContext();
        UniTask SaveCloudContext();
        bool CheckUpdateFromCloud();
        void TransferCloudToLocal();
        bool CheckUpdateFromLocal();
        void TransferLocalToCloud();
        UniTask ClearLocalContext();
        UniTask ClearCloudContext();
    }
}