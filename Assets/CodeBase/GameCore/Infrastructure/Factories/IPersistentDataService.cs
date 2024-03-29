using CodeBase.GameCore.Infrastructure.Services;

namespace CodeBase.GameCore.Infrastructure.Factories
{
    public interface IPersistentDataService : IInitializeable, IService
    {
        void LoadOrDefaultUpgradeModelsFromLocal();
    }
}