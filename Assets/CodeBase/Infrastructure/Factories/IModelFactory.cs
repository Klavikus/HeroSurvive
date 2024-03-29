using CodeBase.Domain.Models;

namespace CodeBase.Infrastructure.Factories
{
    public interface IModelFactory
    {
        UpgradeModel[] CreateUpgradeModels();
    }
}