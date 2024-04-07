using System;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api.Providers;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using UnityEngine;
using IConfigurationProvider = GameCore.Source.Domain.Services.IConfigurationProvider;

namespace GameCore.Source.Controllers.Core.Providers
{
    public class PropertyProvider : IPropertyProvider, IDisposable
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IUpgradeService _upgradeService;
        private readonly IModelProvider _modelProvider;

        private HeroModel _heroModel;
        private PropertiesModel _propertiesModel;

        private MainProperties _baseData;
        private MainProperties _persistentUpgradesData;
        private MainProperties _heroData;
        private MainProperties _resultData;

        public event Action PropertiesUpdated;

        public PropertyProvider(
            IConfigurationProvider configurationProvider,
            IUpgradeService upgradeService,
            IModelProvider modelProvider)
        {
            _configurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _upgradeService = upgradeService ?? throw new ArgumentNullException(nameof(upgradeService));
            _modelProvider = modelProvider ?? throw new ArgumentNullException(nameof(modelProvider));
        }

        public void Initialize()
        {
            _heroModel = _modelProvider.Get<HeroModel>();
            _propertiesModel = _modelProvider.Get<PropertiesModel>();
            
            _baseData = _configurationProvider.BasePropertiesConfig.GetPropertiesData();
            _persistentUpgradesData = _upgradeService.GetUpgradesPropertiesData();
            _heroData = _heroModel.GetMainPropertiesData();
            RecalculateData();

            _upgradeService.Updated += OnUpgradesUpdated;
            _heroModel.Changed += OnHeroChanged;
        }

        public MainProperties GetResultProperties() => _resultData;

        public MainPropertyViewData[] GetResultPropertiesViewData()
        {
            BasePropertiesConfigSO baseConfigurationSo = _configurationProvider.BasePropertiesConfig;
            IReadOnlyList<MainPropertyViewData> baseViews = baseConfigurationSo.PropertiesData;
            MainPropertyViewData[] result = new MainPropertyViewData[baseViews.Count];

            for (int i = 0; i < baseViews.Count; i++)
            {
                result[i] = baseViews[i];
                result[i].Value = _resultData.BaseProperties[result[i].BaseProperty];
            }

            return result;
        }

        public GameObject GetBasePropertyView() =>
            _configurationProvider.BasePropertiesConfig.PropertyView;

        public void Dispose()
        {
            _upgradeService.Updated -= OnUpgradesUpdated;
            _heroModel.Changed -= OnHeroChanged;
        }

        private void OnHeroChanged(HeroData heroData)
        {
            _heroData = _heroModel.GetMainPropertiesData();
            RecalculateData();
        }

        private void OnUpgradesUpdated()
        {
            _persistentUpgradesData = _upgradeService.GetUpgradesPropertiesData();
            RecalculateData();
        }

        private void RecalculateData()
        {
            _resultData = _baseData + _persistentUpgradesData + _heroData;
            _propertiesModel.SetResultProperties(_resultData);
            PropertiesUpdated?.Invoke();
        }
    }
}