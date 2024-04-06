using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Source.Common;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Services;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Services
{
    public class LocalizationService : ILocalizationService
    {
        private readonly IConfigurationProvider _configurationProvider;

        private readonly Dictionary<string, Dictionary<string, string>> _languageDatabase;
        private readonly List<ILocalizable> _localizables;

        public LocalizationService(IConfigurationProvider configurationProvider)
        {
            _configurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _languageDatabase = LocalizationCsvParser.Parse(configurationProvider.LocalizationTablePath);
            _localizables = new List<ILocalizable>();
        }

        public event Action<string> LanguageChanged;
        public event Action Changed;
        public string CurrentLanguage { get; private set; }

        public void Initialize(EnvironmentData environmentData)
        {
            ChangeLanguage(environmentData.Lang);
        }

        public void ChangeLanguage(string languageI18N)
        {
            CurrentLanguage = languageI18N;

            foreach (ILocalizable localizable in _localizables)
                localizable.SetValue(GetLocalizedText(localizable.LocalizationKey));

            LanguageChanged?.Invoke(CurrentLanguage);
            Changed?.Invoke();
        }

        public string GetLocalizedText(string placeHolder) =>
            GetLocalizedText(placeHolder, _languageDatabase);

        public void Register(ILocalizable localizable)
        {
            if (_localizables.Contains(localizable))
                return;

            _localizables.Add(localizable);
            localizable.SetValue(GetLocalizedText(localizable.LocalizationKey));
        }

        public void Unregister(ILocalizable localizable)
        {
            if (_localizables.Contains(localizable) == false)
                return;

            _localizables.Remove(localizable);
        }

        public string GetLocalizedText(TranslatableString[] translatableFullName) =>
            translatableFullName.First(data => data.Language.ToString() == CurrentLanguage).Text;

        private string GetLocalizedText(string key, Dictionary<string, Dictionary<string, string>> dictionary)
        {
            if (dictionary.ContainsKey(key))
            {
                if (dictionary[key].ContainsKey(CurrentLanguage))
                    return dictionary[key][CurrentLanguage].Replace("\\n", "\n");

                Debug.LogWarning($"Can't find language: {CurrentLanguage} for key:{key}");

                return dictionary[key][_configurationProvider.BaseLanguage].Replace("\\n", "\n");
            }

            Debug.LogWarning($"Can't find key: {key}");

            return "NaN";
        }
    }
}