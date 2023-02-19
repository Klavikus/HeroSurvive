using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure.Factories
{
    public interface IPersistentDataService : IService
    {
        void LoadOrDefaultUpgradeModelsFromLocal();
    }
}