using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.Services
{
    public interface IModelProvider : IService
    {
        public GameLoopModel GameLoopModel { get; }
        public UpgradeModel[] UpgradeModels { get; }
        public CurrencyModel CurrencyModel { get; }
        public UserModel UserModel { get; }
    }
}