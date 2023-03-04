using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factories;
using CodeBase.MVVM.Models;

namespace CodeBase.Infrastructure.StateMachine
{
    public class ModelBuilder : IModelBuilder
    {
        private readonly IModelFactory _modelFactory;
        private readonly IAbilityUpgradeService _abilityUpgradeService;

        private static readonly Dictionary<Type, Func<ModelBuilder, object>> BuildStrategyByType =
            new()
            {
                [typeof(HeroModel)] = BuildHeroModel,
                [typeof(PropertiesModel)] = BuildPropertiesModel,
                [typeof(MenuModel)] = BuildMenuModel,
                [typeof(UpgradeModel[])] = BuildUpgradeModels,
                [typeof(CurrencyModel)] = BuildCurrencyModel,
                [typeof(GameLoopModel)] = BuildGameLoopModel,
                [typeof(LevelUpModel)] = BuildLevelUpModel,
            };

        public ModelBuilder(IModelFactory modelFactory, IAbilityUpgradeService abilityUpgradeService)
        {
            _modelFactory = modelFactory;
            _abilityUpgradeService = abilityUpgradeService;
        }

        public T Build<T>() where T : class =>
            BuildStrategyByType[typeof(T)].Invoke(this) as T;

        private static object BuildHeroModel(ModelBuilder builder) => 
            new HeroModel();

        private static object BuildPropertiesModel(ModelBuilder builder) => 
            new PropertiesModel();

        private static object BuildMenuModel(ModelBuilder builder) => 
            new MenuModel();

        private static object BuildUpgradeModels(ModelBuilder builder) => 
           builder._modelFactory.CreateUpgradeModels();

        private static object BuildCurrencyModel(ModelBuilder builder) => 
            new CurrencyModel();

        private static object BuildGameLoopModel(ModelBuilder builder) => 
            new GameLoopModel();

        private static object BuildLevelUpModel(ModelBuilder builder) => 
            new LevelUpModel(builder._abilityUpgradeService);
    }
}