using System;
using System.Collections.Generic;
using Agava.YandexGames;
using CodeBase.Configs;
using CodeBase.Domain.Data;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class SaveLoadService : ISaveLoadService
    {
        private readonly IConfigurationProvider _configurationProvider;
        private Dictionary<string, string> _data = new Dictionary<string, string>();

        public event Action AllLoaded;

        public SaveLoadService(IConfigurationProvider configurationProvider)
        {
            _configurationProvider = configurationProvider;
        }

        public bool ContainData(string dataKey) => _data.ContainsKey(dataKey);

        public string GetData(string dattaKey) => _data[dattaKey];

        public void SaveToData(string dattaKey, string json)
        {
            Debug.Log($"SaveToData {dattaKey} {json}");
            if (ContainData(dattaKey))
                _data[dattaKey] = json;
            else
                _data.Add(dattaKey, json);

            SaveDataToPrefs();
        }

        public void SaveDataToPrefs()
        {
            foreach (KeyValuePair<string, string> keyValuePair in _data)
                PlayerPrefs.SetString(keyValuePair.Key, keyValuePair.Value);

            PlayerPrefs.Save();

            // SaveAllDataOnYandex();
        }

        public void LoadPrefsToData()
        {
            Debug.Log("LoadPrefsToData...");

            foreach (UpgradeData upgradeData in _configurationProvider.UpgradesConfig.UpgradeData)
            {
                string upgradeDataKey = $"{GameConstants.UpgradeModelPrefix}_{upgradeData.Name}";
                SaveToData(upgradeDataKey, PlayerPrefs.GetString(upgradeDataKey, "0"));
            }

            Debug.Log("LoadPrefsToData loaded");
            string currencyValue = PlayerPrefs.HasKey(GameConstants.CurrencyDataKey)
                ? PlayerPrefs.GetString(GameConstants.CurrencyDataKey)
                : "300000";

            SaveToData(GameConstants.CurrencyDataKey, currencyValue);

            AllLoaded?.Invoke();
        }

        public void LoadAllDataFromYandex()
        {
            PlayerAccount.GetPlayerData((OnSuccessCallbackYandexLoad));
        }

        private void SaveAllDataOnYandex()
        {
            PlayerAccount.SetPlayerData(JsonUtility.ToJson(_data));
        }

        private void OnSuccessCallbackYandexLoad(string playerData)
        {
            _data = JsonUtility.FromJson<Dictionary<string, string>>(playerData);
            AllLoaded?.Invoke();
        }
    }
}