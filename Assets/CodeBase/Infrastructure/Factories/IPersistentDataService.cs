using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure.Factories
{
    public interface IPersistentDataService : IInitializeable, IService
    {
        void LoadOrDefaultUpgradeModelsFromLocal();
    }
}