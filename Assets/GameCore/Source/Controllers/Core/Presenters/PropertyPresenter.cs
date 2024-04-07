using System.Collections.Generic;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Enums;
using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class PropertyPresenter : IPresenter
    {
        private readonly IPropertyView _view;
        private readonly IMainPropertiesViewModel _viewModel;
        private readonly IUpgradeDescriptionBuilder _upgradeDescriptionBuilder;
        private readonly ILocalizationService _localizationService;
        private readonly MainPropertyViewData _propertyViewData;

        public PropertyPresenter(
            IPropertyView view,
            IMainPropertiesViewModel viewModel,
            IUpgradeDescriptionBuilder upgradeDescriptionBuilder,
            ILocalizationService localizationService,
            MainPropertyViewData propertyViewData
        )
        {
            _view = view;
            _viewModel = viewModel;
            _upgradeDescriptionBuilder = upgradeDescriptionBuilder;
            _localizationService = localizationService;
            _propertyViewData = propertyViewData;
        }

        public void Enable()
        {
            _view.Icon.sprite = _propertyViewData.Icon;
            _view.ShortName.text = _localizationService.GetLocalizedText(_propertyViewData.TranslatableShortName);
            _view.Value.text = $"{_propertyViewData.Prefix} {_propertyViewData.Value} {_propertyViewData.Postfix}";
            RenderValue(_viewModel.BaseProperties);

            _viewModel.PropertiesChanged += RenderValue;
            _localizationService.Changed += Render;
        }

        public void Disable()
        {
            _viewModel.PropertiesChanged -= RenderValue;
            _localizationService.Changed -= Render;
        }

        private void RenderValue(IReadOnlyDictionary<BaseProperty, float> mainProperties)
        {
            _view.Value.text =
                _upgradeDescriptionBuilder.GetPropertyTextDescription(_propertyViewData,
                    mainProperties[_propertyViewData.BaseProperty]);
        }

        private void Render() =>
            _view.ShortName.text = _localizationService.GetLocalizedText(_propertyViewData.TranslatableShortName);
    }
}