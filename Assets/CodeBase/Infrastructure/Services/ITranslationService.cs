using System;
using CodeBase.Domain;

namespace CodeBase.Infrastructure
{
    public interface ITranslationService : IService
    {
        event Action LocalizationChanged;
        void UpdateLanguage();
        string GetLocalizedText(TranslatableString[] translatableText);
        void ChangeLanguage(string languageKey);
        string GetLocalizedHiddenUser();
    }
}