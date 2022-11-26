using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.Services
{
    public class ModelProvider : IModelProvider
    {
        public ModelProvider(GameLoopModel gameLoopModel,
            UpgradeModel[] upgradeModels,
            CurrencyModel currencyModel,
            UserModel userModel)
        {
            GameLoopModel = gameLoopModel;
            UpgradeModels = upgradeModels;
            CurrencyModel = currencyModel;
            UserModel = userModel;
        }

        public GameLoopModel GameLoopModel { get; }
        public UpgradeModel[] UpgradeModels { get; }
        public CurrencyModel CurrencyModel { get; }
        public UserModel UserModel { get; }
    }
}