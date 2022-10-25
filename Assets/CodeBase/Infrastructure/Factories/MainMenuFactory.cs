using CodeBase.HeroSelection;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PropertiesProviders;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class MainMenuFactory : IMainMenuFactory
    {
        private readonly GameStateMachine _stateMachine;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly IPropertyProvider _propertyProvider;

        private HeroSelectorUI _heroSelectorUI;

        public MainMenuFactory(GameStateMachine stateMachine, IConfigurationProvider configurationProvider, IPropertyProvider propertyProvider)
        {
            _stateMachine = stateMachine;
            _configurationProvider = configurationProvider;
            _propertyProvider = propertyProvider;
        }

        public void Initialization()
        {
            _heroSelectorUI = GameObject.Instantiate(_configurationProvider.GetMainMenuConfig().HeroSelector,
                Vector3.zero, Quaternion.identity);
            _heroSelectorUI.Initialize(_configurationProvider, _propertyProvider);
            // _heroSelectorUI.HeroSelected += OnHeroSelected;
        }

        // private void OnHeroSelected(Hero hero) => _stateMachine.Enter<GameLoopState, Hero>(hero);

        public void ShowMenu() => _heroSelectorUI.Show();
    }
}