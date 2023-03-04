using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.Services
{
    public class ModelProvider : IModelProvider
    {
        public ModelProvider(
            GameLoopModel gameLoopModel,
            UpgradeModel[] upgradeModels,
            CurrencyModel currencyModel, 
            HeroModel heroModel, 
            MenuModel menuModel, 
            PropertiesModel propertiesModel)
        {
            GameLoopModel = gameLoopModel;
            UpgradeModels = upgradeModels;
            CurrencyModel = currencyModel;
            HeroModel = heroModel;
            MenuModel = menuModel;
            PropertiesModel = propertiesModel;
        }

        public GameLoopModel GameLoopModel { get; }
        public UpgradeModel[] UpgradeModels { get; }
        public CurrencyModel CurrencyModel { get; }
        public HeroModel HeroModel { get; }
        public MenuModel MenuModel { get; }
        public PropertiesModel PropertiesModel { get; }
    }
}