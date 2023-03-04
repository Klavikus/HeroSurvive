using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.Services
{
    public interface IModelProvider : IService
    {
        public GameLoopModel GameLoopModel { get; }
        public UpgradeModel[] UpgradeModels { get; }
        public CurrencyModel CurrencyModel { get; }
        public HeroModel HeroModel { get; }
        public MenuModel MenuModel { get; }
        public PropertiesModel PropertiesModel { get; }
    }
}