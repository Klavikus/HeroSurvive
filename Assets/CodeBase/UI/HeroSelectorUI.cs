using System;
using CodeBase.HeroSelection;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PropertiesProviders;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class HeroSelectorUI : MonoBehaviour
    {
        [SerializeField] private GameObject _mainPanel;
        [SerializeField] private HeroViewController _heroViewController;
        [SerializeField] private StartPropertiesUI _startPropertiesUI;

        private IConfigurationProvider _configurationProvider;

        public Action<Hero> HeroSelected;

        public void Initialize(IConfigurationProvider configurationProvider, IPropertyProvider propertyProvider)
        {
            _configurationProvider = configurationProvider;
            _heroViewController.Initialize(configurationProvider);
            _startPropertiesUI.Initialize(configurationProvider.GetBasePropertiesConfig().PropertyView,
                propertyProvider);
        }

        public void Show()
        {
            _mainPanel.SetActive(true);
        }
    }
}