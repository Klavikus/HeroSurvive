using CodeBase.Domain.Data;
using CodeBase.Infrastructure.Services;
using TMPro;
using UnityEngine;

namespace CodeBase.Presentation.Views
{
    public class TranslateSynchronizerTMP : MonoBehaviour
    {
        [SerializeField] private TMP_Text _tmpText;
        [SerializeField] private TranslatableString[] _translatableStrings;

        private ITranslationService _translationService;

        private void Start()
        {
            _translationService = AllServices.Container.AsSingle<ITranslationService>();
            _tmpText.text = _translationService.GetLocalizedText(_translatableStrings);
            _translationService.LocalizationChanged += OnLocalizationChanged;
        }

        private void OnDestroy()
        {
            if (_translationService != null)
                _translationService.LocalizationChanged -= OnLocalizationChanged;
        }

        private void OnLocalizationChanged() =>
            _tmpText.text = _translationService.GetLocalizedText(_translatableStrings);
    }
}