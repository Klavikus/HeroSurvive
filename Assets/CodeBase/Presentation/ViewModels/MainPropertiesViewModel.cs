using System;
using System.Collections.Generic;
using CodeBase.Domain;
using CodeBase.Domain.Enums;
using CodeBase.Domain.Models;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Presentation.ViewModels
{
    public class MainPropertiesViewModel
    {
        private readonly PropertiesModel _propertiesModel;
        private readonly ITranslationService _translationService;

        public event Action<IReadOnlyDictionary<BaseProperty, float>> PropertiesChanged;
        public event Action InvokedRender;

        public IReadOnlyDictionary<BaseProperty, float> BaseProperties => _propertiesModel.MainProperties;

        public MainPropertiesViewModel(PropertiesModel propertiesModel, ITranslationService translationService)
        {
            _propertiesModel = propertiesModel;
            _translationService = translationService;
            _propertiesModel.Changed += OnPropertiesChanged;
            _translationService.LocalizationChanged += OnLocalizationChanged;
        }

        ~MainPropertiesViewModel()
        {
            _propertiesModel.Changed -= OnPropertiesChanged;
            _translationService.LocalizationChanged -= OnLocalizationChanged;
        }

        private void OnLocalizationChanged() => InvokedRender?.Invoke();


        private void OnPropertiesChanged(MainProperties mainProperties)
        {
            PropertiesChanged?.Invoke(BaseProperties);
        }
    }
}