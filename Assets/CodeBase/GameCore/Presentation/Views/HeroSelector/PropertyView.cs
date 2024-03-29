using System.Collections.Generic;
using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Domain.Enums;
using CodeBase.GameCore.Infrastructure.Builders;
using CodeBase.GameCore.Infrastructure.Services;
using CodeBase.GameCore.Presentation.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GameCore.Presentation.Views.HeroSelector
{
    public class PropertyView : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _shortName;
        [SerializeField] private TMP_Text _value;

        private MainPropertyViewData _viewData;

        private MainPropertiesViewModel _viewModel;
        private UpgradeDescriptionBuilder _upgradeDescriptionBuilder;
        private ITranslationService _translationService;

        private void OnEnable()
        {
            if (_viewModel == null)
                return;

            _viewModel.PropertiesChanged += RenderValue;
        }

        private void OnDisable()
        {
            if (_viewModel == null)
                return;

            _viewModel.PropertiesChanged -= RenderValue;
        }

        public void Initialize(MainPropertiesViewModel mainPropertiesViewModel, MainPropertyViewData viewData,
            UpgradeDescriptionBuilder upgradeDescriptionBuilder)
        {
            _translationService = AllServices.Container.AsSingle<ITranslationService>();
            _viewModel = mainPropertiesViewModel;
            _viewData = viewData;
            _upgradeDescriptionBuilder = upgradeDescriptionBuilder;
            _image.sprite = _viewData.Icon;
            _shortName.text = _translationService.GetLocalizedText(_viewData.TranslatableShortName);
            _value.text = $"{_viewData.Prefix} {_viewData.Value} {_viewData.Postfix}";
            RenderValue(mainPropertiesViewModel.BaseProperties);
            _viewModel.PropertiesChanged += RenderValue;
            _viewModel.InvokedRender += Render;
        }

        private void Render() =>
            _shortName.text = _translationService.GetLocalizedText(_viewData.TranslatableShortName);

        private void RenderValue(IReadOnlyDictionary<BaseProperty, float> mainProperties)
        {
            _value.text =
                _upgradeDescriptionBuilder.GetPropertyTextDescription(_viewData,
                    mainProperties[_viewData.BaseProperty]);
        }
    }
}