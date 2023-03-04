using CodeBase.Configs;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.Views;

namespace CodeBase.Infrastructure.Factories
{
    public class PersistentDataService : IPersistentDataService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IProvider _modelProvider;


        public PersistentDataService(IConfigurationProvider configurationProvider,
            ISaveLoadService saveLoadService,
            IProvider modelProvider)
        {
            _configurationProvider = configurationProvider;
            _saveLoadService = saveLoadService;
            _modelProvider = modelProvider;
        }

        public void Initialize()
        {
            _saveLoadService.AllLoaded += SaveLoadServiceOnAllLoaded;

            _modelProvider.Get<CurrencyModel>().CurrencyChanged += SaveCurrency;
            foreach (UpgradeModel upgradeModel in _modelProvider.Get<UpgradeModel[]>())
                upgradeModel.LevelChanged += SaveUpgrade;
        }

        public void LoadOrDefaultUpgradeModelsFromLocal()
        {
            _saveLoadService.LoadPrefsToData();
        }

        private void SaveLoadServiceOnAllLoaded()
        {
            UpgradeModel[] result = _modelProvider.Get<UpgradeModel[]>();

            for (var i = 0; i < result.Length; i++)
            {
                string dattaKey = $"{GameConstants.UpgradeModelPrefix}_{result[i].Data.KeyName}";

                if (_saveLoadService.ContainData(dattaKey))
                {
                    if (int.TryParse(_saveLoadService.GetData(dattaKey), out int value))
                        result[i].SetLevel(value);
                }
            }

            if (int.TryParse(_saveLoadService.GetData(GameConstants.CurrencyDataKey), out int loadedCurrency))
                _modelProvider.Get<CurrencyModel>().SetAmount(loadedCurrency);
        }

        private void SaveUpgrade(UpgradeModel upgradeModel)
        {
            string dattaKey = $"{GameConstants.UpgradeModelPrefix}_{upgradeModel.Data.KeyName}";
            _saveLoadService.SaveToData(dattaKey, upgradeModel.CurrentLevel.ToString());
        }

        private void SaveCurrency(int currentCurrency)
        {
            string dataKey = $"{GameConstants.CurrencyDataKey}";
            _saveLoadService.SaveToData(dataKey, currentCurrency.ToString());
        }
    }
}