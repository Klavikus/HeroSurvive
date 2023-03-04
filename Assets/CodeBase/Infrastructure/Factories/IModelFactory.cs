using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.Factories
{
    public interface IModelFactory
    {
        UpgradeModel[] CreateUpgradeModels();
    }
}