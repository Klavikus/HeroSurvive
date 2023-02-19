using System;
using System.Collections.Generic;
using Agava.YandexGames;
using CodeBase.Configs;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enums;

namespace CodeBase.Infrastructure.Services
{
    public class TranslationService : ITranslationService
    {
        private static readonly Dictionary<string, Language> I18nToLanguage = new()
        {
            {"en", Language.en},
            {"ru", Language.ru},
            {"tr", Language.tr},
        };

        private static readonly Dictionary<Language, string> LanguageToHiddenUser = new()
        {
            {Language.en, "Mysterious person"},
            {Language.ru, "Таинственная личность"},
            {Language.tr, "Gizemli kişilik"},
        };

        private Language _currentLanguage;

        public event Action LocalizationChanged;

        public void UpdateLanguage()
        {
#if !UNITY_WEBGL || UNITY_EDITOR
            _currentLanguage = I18nToLanguage[GameConstants.BaseLanguage];
            return;
#endif
            string yandexLang = YandexGamesSdk.Environment.i18n.lang;
            if (I18nToLanguage.ContainsKey(yandexLang) == false)
                yandexLang = GameConstants.BaseLanguage;
            ChangeLanguage(yandexLang);
        }

        public string GetLocalizedText(TranslatableString[] translatableText)
        {
            foreach (TranslatableString translatableString in translatableText)
                if (translatableString.Language == _currentLanguage)
                    return translatableString.Text;

            return string.Empty;
        }

        public void ChangeLanguage(string languageKey)
        {
            _currentLanguage = I18nToLanguage[languageKey];
            LocalizationChanged?.Invoke();
        }

        public string GetLocalizedHiddenUser() => LanguageToHiddenUser[_currentLanguage];
    }
}