using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Domain.Data;
using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Factories
{
    public class PropertyPresenterFactory : IPropertyPresenterFactory
    {
        private readonly IMainPropertiesViewModel _mainPropertiesViewModel;
        private readonly IUpgradeDescriptionBuilder _upgradeDescriptionBuilder;
        private readonly ILocalizationService _localizationService;

        public PropertyPresenterFactory(
            IMainPropertiesViewModel mainPropertiesViewModel,
            IUpgradeDescriptionBuilder upgradeDescriptionBuilder,
            ILocalizationService localizationService)
        {
            _mainPropertiesViewModel = mainPropertiesViewModel;
            _upgradeDescriptionBuilder = upgradeDescriptionBuilder;
            _localizationService = localizationService;
        }

        public IPresenter Create(IPropertyView view, MainPropertyViewData propertyViewData) =>
            new PropertyPresenter(
                view,
                _mainPropertiesViewModel,
                _upgradeDescriptionBuilder,
                _localizationService,
                propertyViewData);
    }
}