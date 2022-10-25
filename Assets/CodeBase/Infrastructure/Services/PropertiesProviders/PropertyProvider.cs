using System;
using System.Collections.Generic;
using CodeBase.HeroSelection;
using CodeBase.Infrastructure.Services.HeroSelectionService;
using CodeBase.Infrastructure.Services.UpgradeService;

namespace CodeBase.Infrastructure.Services.PropertiesProviders
{
    public class PropertyProvider : IPropertyProvider
    {
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IUpgradeService _upgradeService;
        private readonly IHeroSelectionService _heroSelectionService;

        private MainProperties _baseData;
        private MainProperties _upgradesData;
        private MainProperties _heroData;
        private MainProperties _resultData;

        public event Action PropertiesUpdated;

        public PropertyProvider(
            IConfigurationProvider configurationProvider,
            IUpgradeService upgradeService,
            IHeroSelectionService heroSelectionService)
        {
            _configurationProvider = configurationProvider;
            _upgradeService = upgradeService;
            _heroSelectionService = heroSelectionService;
        }

        public void Initialize()
        {
            _baseData = _configurationProvider.GetBasePropertiesConfig().GetPropertiesData();
            _upgradesData = _upgradeService.GetUpgradesPropertiesData();
            _heroData = _heroSelectionService.GetHeroPropertiesData();
            RecalculateData();
            
            _upgradeService.Updated += OnUpgradesUpdated;
            _heroSelectionService.HeroSelected += OnHeroSelected;
        }

        public MainProperties GetResultProperties() => _resultData;

        public MainPropertyViewData[] GetResultPropertiesViewData()
        {
            BasePropertiesConfigSO baseConfigurationSO = _configurationProvider.GetBasePropertiesConfig();
            IReadOnlyList<MainPropertyViewData> baseViews = baseConfigurationSO.PropertiesData;
            MainPropertyViewData[] result = new MainPropertyViewData[baseViews.Count];
          
            for (int i = 0; i < baseViews.Count; i++)
            {
                result[i] = baseViews[i];
                result[i].Value = _resultData.BaseProperties[result[i].BaseProperty];
            }

            return result;
        }

        public PropertyView GetBasePropertyView() => _configurationProvider.GetBasePropertiesConfig().PropertyView;

        private void OnHeroSelected()
        {
            _heroData = _heroSelectionService.GetHeroPropertiesData();
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
            PropertiesUpdated?.Invoke();
        }
    }
}