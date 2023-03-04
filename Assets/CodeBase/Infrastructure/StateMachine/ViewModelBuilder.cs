using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.UpgradeService;
using CodeBase.MVVM.ViewModels;

namespace CodeBase.Infrastructure.StateMachine
{
    class ViewModelBuilder : IViewModelBuilder
    {
        private static readonly Dictionary<Type, Func<ViewModelBuilder, Object>> BuildStrategyByType =
            new()
            {
                [typeof(HeroSelectorViewModel)] = BuildHeroSelectorViewModel,
                [typeof(MainPropertiesViewModel)] = BuildMainPropertiesViewModel,
                [typeof(MenuViewModel)] = BuildMenuViewModel,
                [typeof(UpgradeViewModel)] = BuildUpgradeViewModel,
                [typeof(CurrencyViewModel)] = BuildCurrencyViewModel,
            };

        private readonly IModelProvider _modelProvider;
        private readonly ITranslationService _translationService;
        private readonly IUpgradeService _upgradeService;

        public ViewModelBuilder(
            IModelProvider modelProvider,
            ITranslationService translationService,
            IUpgradeService upgradeService)
        {
            _modelProvider = modelProvider;
            _translationService = translationService;
            _upgradeService = upgradeService;
        }

        public TViewModel Build<TViewModel>() where TViewModel : class =>
            BuildStrategyByType[typeof(TViewModel)].Invoke(this) as TViewModel;

        private static HeroSelectorViewModel BuildHeroSelectorViewModel(ViewModelBuilder builder) =>
            new HeroSelectorViewModel(builder._modelProvider.HeroModel, builder._modelProvider.MenuModel,
                builder._modelProvider.GameLoopModel,
                builder._translationService);

        private static MainPropertiesViewModel BuildMainPropertiesViewModel(ViewModelBuilder builder) =>
            new MainPropertiesViewModel(builder._modelProvider.PropertiesModel, builder._translationService);

        private static MenuViewModel BuildMenuViewModel(ViewModelBuilder builder) =>
            new MenuViewModel(builder._modelProvider.MenuModel);

        private static UpgradeViewModel BuildUpgradeViewModel(ViewModelBuilder builder) =>
            new UpgradeViewModel(builder._modelProvider.UpgradeModels, builder._modelProvider.CurrencyModel,
                builder._upgradeService);

        private static CurrencyViewModel BuildCurrencyViewModel(ViewModelBuilder builder) =>
            new CurrencyViewModel(builder._modelProvider.CurrencyModel);
    }
}