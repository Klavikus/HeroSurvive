using System;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Controllers.Core.Services;
using GameCore.Source.Controllers.Core.ViewModels;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Core.Services.DI;
using GameCore.Source.Presentation.Api.MainMenu.HeroSelector;
using GameCore.Source.Presentation.Core.Factories;
using GameCore.Source.Presentation.Core.GameLoop;
using GameCore.Source.Presentation.Core.MainMenu;
using GameCore.Source.Presentation.Core.MainMenu.Upgrades;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.Common.WindowFsm.Runtime.Implementation;
using UnityEngine;

namespace GameCore.Source.Application.CompositionRoots
{
    public class MainMenuCompositionRoot : SceneCompositionRoot
    {
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private MVPLeaderBoardsView _leaderBoardsView;
        [SerializeField] private LocalizationSystemView _localizationSystemView;
        [SerializeField] private UpgradesSelectorView _upgradesSelectorView;
        [SerializeField] private CurrencyView _currencyView;
        [SerializeField] private UpgradeFocusView _upgradeFocusView;
        [SerializeField] private HeroSelectorView _heroSelectorView;

        public override void Initialize(ServiceContainer serviceContainer)
        {
            Dictionary<Type, IWindow> windows = new Dictionary<Type, IWindow>()
            {
                [typeof(MainMenuWindow)] = new MainMenuWindow(),
                [typeof(LeaderBoardWindow)] = new LeaderBoardWindow(),
                [typeof(UpgradeSelectorWindow)] = new UpgradeSelectorWindow(),
                [typeof(HeroSelectorWindow)] = new HeroSelectorWindow(),
            };

            WindowFsm<MainMenuWindow> windowFsm = new WindowFsm<MainMenuWindow>(windows);

            IConfigurationProvider configurationProvider = serviceContainer.Single<IConfigurationProvider>();
            IGameStateMachine gameStateMachine = serviceContainer.Single<IGameStateMachine>();
            ILocalizationService localizationService = serviceContainer.Single<ILocalizationService>();

            IModelProvider modelProvider = serviceContainer.Single<IModelProvider>();
            IAudioPlayerService audioPlayerService = serviceContainer.Single<IAudioPlayerService>();
            IPropertyProvider propertyProvider = serviceContainer.Single<IPropertyProvider>();

            UpgradeModel[] upgradeModels = modelProvider.Get<UpgradeModel[]>();
            CurrencyModel currencyModel = modelProvider.Get<CurrencyModel>();
            HeroModel heroModel = modelProvider.Get<HeroModel>();
            PropertiesModel propertiesModel = modelProvider.Get<PropertiesModel>();
            GameLoopModel gameLoopModel = new GameLoopModel();

            propertyProvider.Initialize();

            currencyModel.Add(10000);

            HeroSelectorViewModel heroSelectorViewModel = new(
                heroModel,
                gameLoopModel,
                configurationProvider,
                audioPlayerService);

            IMainPropertiesViewModel mainPropertiesViewModel = new MainPropertiesViewModel(propertiesModel);

            IUpgradeService upgradeService = new UpgradeService(upgradeModels);

            UpgradeDescriptionBuilder descriptionBuilder = new(configurationProvider, localizationService);

            PersistentUpgradeService persistentUpgradeService = new(
                upgradeModels,
                currencyModel,
                upgradeService,
                audioPlayerService
            );
            PersistentUpgradeLevelViewFactory persistentUpgradeLevelViewFactory = new(configurationProvider);

            PersistentUpgradePresenterFactory persistentUpgradePresenterFactory =
                new(persistentUpgradeService, persistentUpgradeLevelViewFactory,
                    localizationService);

            SelectableHeroPresenterFactory selectableHeroPresenterFactory = new(heroSelectorViewModel);
            PropertyPresenterFactory propertyPresenterFactory = new(
                mainPropertiesViewModel,
                descriptionBuilder,
                localizationService);

            PersistentUpgradeViewFactory persistentUpgradeViewFactory =
                new(configurationProvider, persistentUpgradePresenterFactory.Create);

            SelectableHeroViewFactory selectableHeroViewFactory =
                new(configurationProvider, selectableHeroPresenterFactory.Create);

            PropertyViewFactory propertyViewFactory =
                new(configurationProvider, propertyPresenterFactory.Create);

            LocalizationSystemPresenter localizationSystemPresenter = new(_localizationSystemView, localizationService);
            _localizationSystemView.Construct(localizationSystemPresenter);

            MainMenuPresenter mainMenuPresenter = new(windowFsm, _mainMenuView);
            _mainMenuView.Construct(mainMenuPresenter);

            LeaderBoardPresenter leaderBoardPresenter = new(_leaderBoardsView, windowFsm);
            _leaderBoardsView.Construct(leaderBoardPresenter);

            CurrencyPresenter currencyPresenter = new(_currencyView, currencyModel);
            _currencyView.Construct(currencyPresenter);

            UpgradeSelectorPresenter upgradeSelectorPresenter = new(
                windowFsm,
                _upgradesSelectorView,
                persistentUpgradeService,
                persistentUpgradeViewFactory);
            _upgradesSelectorView.Construct(upgradeSelectorPresenter);

            UpgradeFocusPresenter upgradeFocusPresenter = new(
                _upgradeFocusView,
                persistentUpgradeService,
                descriptionBuilder,
                persistentUpgradeLevelViewFactory,
                localizationService);
            _upgradeFocusView.Construct(upgradeFocusPresenter);

            HeroSelectorPresenter heroSelectorPresenter = new(
                windowFsm,
                _heroSelectorView,
                heroSelectorViewModel,
                localizationService,
                gameStateMachine);
            _heroSelectorView.Construct(heroSelectorPresenter);

            foreach (ISelectableHeroView selectableHeroView in selectableHeroViewFactory.Create())
            {
                selectableHeroView.Transform.SetParent(_heroSelectorView.HeroViewsContainer);
                selectableHeroView.Transform.localScale = Vector3.one;
            }

            foreach (IPropertyView propertyView in propertyViewFactory.Create())
            {
                propertyView.Transform.SetParent(_heroSelectorView.PropertiesViewContainer);
                propertyView.Transform.localScale = Vector3.one;
            }
        }
    }
}