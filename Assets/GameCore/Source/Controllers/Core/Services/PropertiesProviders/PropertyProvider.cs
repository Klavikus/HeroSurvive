using System;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Configs;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using UnityEngine;
using IConfigurationProvider = GameCore.Source.Domain.Services.IConfigurationProvider;

namespace GameCore.Source.Controllers.Core.Services.PropertiesProviders
{
    public class PropertyProvider : IPropertyProvider, IDisposable
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

        public PropertyProvider(
            IConfigurationProvider configurationProvider,
            IUpgradeService upgradeService,
            HeroModel heroModel,
            PropertiesModel propertiesModel)
        {
            _configurationProvider =
                configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
            _upgradeService = upgradeService ?? throw new ArgumentNullException(nameof(upgradeService));
            _heroModel = heroModel ?? throw new ArgumentNullException(nameof(heroModel));
            _propertiesModel = propertiesModel ?? throw new ArgumentNullException(nameof(propertiesModel));
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