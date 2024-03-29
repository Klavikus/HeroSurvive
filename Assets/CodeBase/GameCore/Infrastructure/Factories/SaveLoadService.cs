using System;
using System.Collections.Generic;
using System.Globalization;
using Agava.YandexGames;
using CodeBase.GameCore.Configs;
using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.GameCore.Infrastructure.Factories
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

        public string GetData(string dattaKey, string defaultValue) => 
            _data.ContainsKey(dattaKey) ? _data[dattaKey] : defaultValue;

        public void SaveToData(string dattaKey, string json)
        {
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
        }

        public void LoadPrefsToData()
        {
            foreach (UpgradeData upgradeData in _configurationProvider.UpgradesConfig.UpgradeData)
            {
                string upgradeDataKey = $"{GameConstants.UpgradeModelPrefix}_{upgradeData.KeyName}";
                SaveToData(upgradeDataKey, PlayerPrefs.GetString(upgradeDataKey, "0"));
            }

            string currencyValue = PlayerPrefs.HasKey(GameConstants.CurrencyDataKey)
                ? PlayerPrefs.GetString(GameConstants.CurrencyDataKey)
                : "300000";
            SaveToData(GameConstants.CurrencyDataKey, currencyValue);

            string userName = PlayerPrefs.HasKey(GameConstants.UserNameDataKey)
                ? PlayerPrefs.GetString(GameConstants.UserNameDataKey)
                : GameConstants.BaseUserName;
            SaveToData(GameConstants.UserNameDataKey, userName);

            string musicVolume = PlayerPrefs.HasKey(GameConstants.MusicVolume)
                ? PlayerPrefs.GetString(GameConstants.MusicVolume)
                : GameConstants.BaseMusicVolume.ToString(CultureInfo.InvariantCulture);
            SaveToData(GameConstants.MusicVolume, musicVolume);

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