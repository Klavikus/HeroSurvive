using CodeBase.GameCore.Domain.Models;

namespace CodeBase.GameCore.Infrastructure.Factories
{
    public interface IModelFactory
    {
        UpgradeModel[] CreateUpgradeModels();
    }
}