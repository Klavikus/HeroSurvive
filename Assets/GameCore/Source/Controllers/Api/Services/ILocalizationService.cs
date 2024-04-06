using System;
using GameCore.Source.Common;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface ILocalizationService
    {
        event Action<string> LanguageChanged;
        event Action Changed;
        string CurrentLanguage { get; }
        void Initialize(EnvironmentData environmentData);
        void ChangeLanguage(string languageI18N);
        string GetLocalizedText(string placeHolder);
        void Register(ILocalizable localizable);
        void Unregister(ILocalizable localizable);
        string GetLocalizedText(TranslatableString[] translatableFullName);
    }
}