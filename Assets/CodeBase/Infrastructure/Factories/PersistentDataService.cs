using CodeBase.Configs;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class PersistentDataService : IPersistentDataService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly ISaveLoadService _saveLoadService;
        private readonly ModelProvider _modelProvider;

        public PersistentDataService(IConfigurationProvider configurationProvider,
            ISaveLoadService saveLoadService,
            ModelProvider modelProvider)
        {
            _configurationProvider = configurationProvider;
            _saveLoadService = saveLoadService;
            _modelProvider = modelProvider;
            _saveLoadService.AllLoaded += SaveLoadServiceOnAllLoaded;

            _modelProvider.CurrencyModel.CurrencyChanged += SaveCurrency;
            foreach (UpgradeModel upgradeModel in _modelProvider.UpgradeModels)
                upgradeModel.LevelChanged += SaveUpgrade;
            _modelProvider.UserModel.NameChanged += SaveUserName;
        }

        public void LoadOrDefaultUpgradeModels()
        {
            _saveLoadService.LoadPrefsToData();
            // _saveLoadService.LoadAllDataFromYandex();
        }

        private void SaveLoadServiceOnAllLoaded()
        {
            UpgradeModel[] result = _modelProvider.UpgradeModels;

            for (var i = 0; i < result.Length; i++)
            {
                string dattaKey = $"{GameConstants.UpgradeModelPrefix}_{result[i].Data.Name}";

                if (_saveLoadService.ContainData(dattaKey))
                {
                    Debug.Log(dattaKey + " " + _saveLoadService.GetData(dattaKey));
                    if (int.TryParse(_saveLoadService.GetData(dattaKey), out int value))
                        result[i].SetLevel(value);
                }
            }

            if (int.TryParse(_saveLoadService.GetData(GameConstants.CurrencyDataKey), out int loadedCurrency))
                _modelProvider.CurrencyModel.SetAmount(loadedCurrency);
        }

        private void SaveUpgrade(UpgradeModel upgradeModel)
        {
            string dattaKey = $"{GameConstants.UpgradeModelPrefix}_{upgradeModel.Data.Name}";
            _saveLoadService.SaveToData(dattaKey, upgradeModel.CurrentLevel.ToString());
        }

        private void SaveCurrency(int currentCurrency)
        {
            string dataKey = $"{GameConstants.CurrencyDataKey}";
            _saveLoadService.SaveToData(dataKey, currentCurrency.ToString());
        }

        private void SaveUserName(string userName)
        {
            string dataKey = $"{GameConstants.UserNameDataKey}";
            _saveLoadService.SaveToData(dataKey, userName);
        }
    }
}