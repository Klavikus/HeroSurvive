using CodeBase.Infrastructure.Services;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase.Helpers
{
    public class TranslateTestHelper : MonoBehaviour
    {
        private static string[] I18nToLeanLanguage = {
            "en",
            "ru",
            "tr",
        };

        private int _currentLanguageId;

        [Button(nameof(ChangeLanguage))]
        private void ChangeLanguage()
        {
            if (++_currentLanguageId == I18nToLeanLanguage.Length)
                _currentLanguageId = 0;

            AllServices.Container.AsSingle<ITranslationService>().ChangeLanguage(I18nToLeanLanguage[_currentLanguageId]);
        }
    }
}