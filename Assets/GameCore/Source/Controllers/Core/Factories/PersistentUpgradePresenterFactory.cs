using System;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Factories;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Controllers.Core.Presenters.MainMenu;
using GameCore.Source.Domain.Data;
using GameCore.Source.Presentation.Api;
using GameCore.Source.Presentation.Api.Factories;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Factories
{
    public class PersistentUpgradePresenterFactory : IPersistentUpgradePresenterFactory
    {
        private readonly IPersistentUpgradeService _persistentUpgradeService;
        private readonly IPersistentUpgradeLevelViewFactory _persistentUpgradeLevelViewFactory;
        private readonly ILocalizationService _localizationService;

        public PersistentUpgradePresenterFactory(
            IPersistentUpgradeService persistentUpgradeService,
            IPersistentUpgradeLevelViewFactory persistentUpgradeLevelViewFactory,
            ILocalizationService localizationService)
        {
            _persistentUpgradeService = persistentUpgradeService ??
                                        throw new ArgumentNullException(nameof(persistentUpgradeService));
            _persistentUpgradeLevelViewFactory = persistentUpgradeLevelViewFactory ??
                                                 throw new ArgumentNullException(
                                                     nameof(persistentUpgradeLevelViewFactory));
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
        }

        public IPresenter Create(UpgradeData upgradeData, IPersistentUpgradeView view) =>
            new PersistentUpgradePresenter(
                view,
                _persistentUpgradeService,
                _persistentUpgradeLevelViewFactory,
                _localizationService,
                upgradeData);
    }
}