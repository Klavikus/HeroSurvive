using System;
using GameCore.Source.Common;
using GameCore.Source.Common.Localization;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class LocalizationSystemPresenter : IPresenter
    {
        private readonly ILocalizationSystemView _view;
        private readonly ILocalizationService _localizationService;

        public LocalizationSystemPresenter(
            ILocalizationSystemView view,
            ILocalizationService localizationService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
        }

        public void Enable()
        {
            foreach (ILocalizable localizable in _view.Localizables) 
                _localizationService.Register(localizable);
        }

        public void Disable()
        {
            foreach (ILocalizable localizable in _view.Localizables) 
                _localizationService.Unregister(localizable);
        }
    }
}