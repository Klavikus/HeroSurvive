using System;
using System.Collections.Generic;
using CodeBase.Configs;
using CodeBase.Domain;
using CodeBase.Infrastructure.Services;
using CodeBase.Presentation;

namespace CodeBase.Infrastructure
{
    public class ViewModelBuilder : IViewModelBuilder
    {
        private static readonly Dictionary<Type, Func<ViewModelBuilder, object>> BuildStrategyByType =
            new()
            {
                [typeof(HeroSelectorViewModel)] = BuildHeroSelectorViewModel,
                [typeof(MainPropertiesViewModel)] = BuildMainPropertiesViewModel,
                [typeof(MenuViewModel)] = BuildMenuViewModel,
                [typeof(UpgradeViewModel)] = BuildUpgradeViewModel,
                [typeof(CurrencyViewModel)] = BuildCurrencyViewModel,
                [typeof(LeaderBoardsViewModel)] = BuildLeaderBoardsViewModel,
                [typeof(GameLoopViewModel)] = BuildGameLoopViewModel,
                [typeof(LevelUpViewModel)] = BuildLevelUpViewModel,
            };

        private readonly IConfigurationProvider _configurationProvider;
        private readonly IModelProvider _modelProvider;
        private readonly ITranslationService _translationService;
        private readonly IUpgradeService _upgradeService;
        private readonly IAuthorizeService _authorizeService;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly ILeveCompetitionService _leveCompetitionService;
        private readonly PlayerEventHandler _playerEventHandler;
        private readonly IAdsProvider _adsProvider;
        private readonly IAbilityUpgradeService _abilityUpgradeService;
        private readonly IAudioPlayerService _sfxService;

        public ViewModelBuilder(
            IConfigurationProvider configurationProvider,
            IModelProvider modelProvider,
            ITranslationService translationService,
            IUpgradeService upgradeService,
            IAuthorizeService authorizeService,
            ICoroutineRunner coroutineRunner,
            ILeveCompetitionService leveCompetitionService,
            PlayerEventHandler playerEventHandler,
            IAdsProvider adsProvider,
            IAbilityUpgradeService abilityUpgradeService,
            IAudioPlayerService sfxService)
        {
            _configurationProvider = configurationProvider;
            _modelProvider = modelProvider;
            _translationService = translationService;
            _upgradeService = upgradeService;
            _authorizeService = authorizeService;
            _coroutineRunner = coroutineRunner;
            _leveCompetitionService = leveCompetitionService;
            _playerEventHandler = playerEventHandler;
            _adsProvider = adsProvider;
            _abilityUpgradeService = abilityUpgradeService;
            _sfxService = sfxService;
        }

        public T Build<T>() where T : class =>
            BuildStrategyByType[typeof(T)].Invoke(this) as T;

        private static HeroSelectorViewModel BuildHeroSelectorViewModel(ViewModelBuilder builder) =>
            new HeroSelectorViewModel(
                builder._modelProvider.Get<HeroModel>(),
                builder._modelProvider.Get<MenuModel>(),
                builder._modelProvider.Get<GameLoopModel>(),
                builder._translationService,
                builder._configurationProvider);

        private static MainPropertiesViewModel BuildMainPropertiesViewModel(ViewModelBuilder builder) =>
            new MainPropertiesViewModel(
                builder._modelProvider.Get<PropertiesModel>(),
                builder._translationService);

        private static MenuViewModel BuildMenuViewModel(ViewModelBuilder builder) =>
            new MenuViewModel(builder._modelProvider.Get<MenuModel>());

        private static UpgradeViewModel BuildUpgradeViewModel(ViewModelBuilder builder) =>
            new UpgradeViewModel(
                builder._modelProvider.Get<UpgradeModel[]>(),
                builder._modelProvider.Get<CurrencyModel>(),
                builder._upgradeService, 
                builder._sfxService);

        private static CurrencyViewModel BuildCurrencyViewModel(ViewModelBuilder builder) =>
            new CurrencyViewModel(builder._modelProvider.Get<CurrencyModel>());

        private static LeaderBoardsViewModel BuildLeaderBoardsViewModel(ViewModelBuilder builder) =>
            new LeaderBoardsViewModel(
                builder._authorizeService,
                new[] {new LeaderBoard(GameConstants.StageTotalKillsLeaderBoardKey)},
                builder._coroutineRunner,
                builder._modelProvider.Get<MenuModel>());

        private static GameLoopViewModel BuildGameLoopViewModel(ViewModelBuilder builder) =>
            new GameLoopViewModel(
                builder._modelProvider.Get<GameLoopModel>(),
                builder._leveCompetitionService,
                builder._playerEventHandler,
                builder._adsProvider,
                builder._sfxService);

        private static LevelUpViewModel BuildLevelUpViewModel(ViewModelBuilder builder) =>
            new LevelUpViewModel(
                builder._modelProvider.Get<LevelUpModel>(),
                builder._abilityUpgradeService,
                builder._adsProvider);
    }
}