using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.Services
{
    public class ModelProvider : IModelProvider
    {
        public ModelProvider(GameLoopModel gameLoopModel,
            UpgradeModel[] upgradeModels,
            CurrencyModel currencyModel)
        {
            GameLoopModel = gameLoopModel;
            UpgradeModels = upgradeModels;
            CurrencyModel = currencyModel;
        }

        public GameLoopModel GameLoopModel { get; }
        public UpgradeModel[] UpgradeModels { get; }
        public CurrencyModel CurrencyModel { get; }
    }
}