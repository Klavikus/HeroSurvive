namespace CodeBase.Infrastructure
{
    public interface IPersistentDataService : IInitializeable, IService
    {
        void LoadOrDefaultUpgradeModelsFromLocal();
    }
}