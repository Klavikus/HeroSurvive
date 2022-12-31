using System;
using CodeBase.Domain.Data;

namespace CodeBase.Infrastructure.Services
{
    public interface ITranslationService : IService
    {
        event Action LocalizationChanged;
        void UpdateLanguage();
        string GetLocalizedText(TranslatableString[] translatableText);
        void ChangeLanguage(string languageKey);
    }
}