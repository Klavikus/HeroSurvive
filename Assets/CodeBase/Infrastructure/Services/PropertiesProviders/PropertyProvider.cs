using System;
using System.Collections.Generic;
using CodeBase.Configs;
using CodeBase.Domain;
using CodeBase.Domain.Data;
using CodeBase.Infrastructure.Services.UpgradeService;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.Views;
using CodeBase.MVVM.Views.HeroSelector;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.PropertiesProviders
{
    public class PropertyProvider : IPropertyProvider
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IUpgradeService _upgradeService;
        private readonly HeroModel _heroModel;
        private readonly PropertiesModel _propertiesModel;

        private MainProperties _baseData;
        private MainProperties _upgradesData;
        private MainProperties _heroData;
        private MainProperties _resultData;

        public event Action PropertiesUpdated;

        public PropertyProvider(IConfigurationProvider configurationProvider,
            IUpgradeService upgradeService,
            HeroModel heroModel,
            PropertiesModel propertiesViewModel)
        {
            _configurationProvider = configurationProvider;
            _upgradeService = upgradeService;
            _heroModel = heroModel;
            _propertiesModel = propertiesViewModel;
        }

        public void Initialize()
        {
            _baseData = _configurationProvider.BasePropertiesConfig.GetPropertiesData();
            _upgradesData = _upgradeService.GetUpgradesPropertiesData();
            _heroData = _heroModel.GetMainPropertiesData();
            RecalculateData();
            
            _upgradeService.Updated += OnUpgradesUpdated;
            _heroModel.Changed += OnHeroChanged;
        }

        public MainProperties GetResultProperties() => _resultData;

        public MainPropertyViewData[] GetResultPropertiesViewData()
        {
            BasePropertiesConfigSO baseConfigurationSO = _configurationProvider.BasePropertiesConfig;
            IReadOnlyList<MainPropertyViewData> baseViews = baseConfigurationSO.PropertiesData;
            MainPropertyViewData[] result = new MainPropertyViewData[baseViews.Count];
          
            for (int i = 0; i < baseViews.Count; i++)
            {
                result[i] = baseViews[i];
                result[i].Value = _resultData.BaseProperties[result[i].BaseProperty];
            }

            return result;
        }

        public PropertyView GetBasePropertyView() => _configurationProvider.BasePropertiesConfig.PropertyView;

        private void OnHeroChanged(HeroData heroData)
        {
            _heroData = _heroModel.GetMainPropertiesData();
            RecalculateData();
        }

        private void OnUpgradesUpdated()
        {
            Debug.Log("OnUpgradesUpdated provider");
            _upgradesData = _upgradeService.GetUpgradesPropertiesData();
            RecalculateData();
        }

        private void RecalculateData()
        {
            _resultData = _baseData + _upgradesData + _heroData;
            _propertiesModel.SetResultProperties(_resultData);
            PropertiesUpdated?.Invoke();
        }
    }
}