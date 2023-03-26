using CodeBase.Domain;

namespace CodeBase.Infrastructure
{
    public interface IModelFactory
    {
        UpgradeModel[] CreateUpgradeModels();
    }
}