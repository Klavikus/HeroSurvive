using System;
using CodeBase.GameCore.Domain.Data;

namespace CodeBase.GameCore.Infrastructure.Services
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