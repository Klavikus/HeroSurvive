using CodeBase.HeroSelection;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Infrastructure.Factories
{
    public class MainMenuFactory
    {
        private readonly GameStateMachine _stateMachine;
        private readonly MainMenuConfiguration _configuration;

        private HeroSelectorUI _heroSelectorUI;
        
        public MainMenuFactory(GameStateMachine stateMachine, MainMenuConfiguration configuration)
        {
            _stateMachine = stateMachine;
            _configuration = configuration;
        }

        public void Initialization()
        {
            _heroSelectorUI = GameObject.Instantiate(_configuration.HeroSelector, Vector3.zero, Quaternion.identity);
            _heroSelectorUI.HeroSelected += OnHeroSelected;
        }

        private void OnHeroSelected(Hero hero)
        {
            _stateMachine.Enter<GameLoopState, Hero>(hero);
        }

        public void ShowMenu()
        {
            _heroSelectorUI.Show();
        }
    }

    [CreateAssetMenu(menuName = "Create MainMenuConfiguration", fileName = "MainMenuConfiguration", order = 0)]
    public class MainMenuConfiguration : ScriptableObject
    {
        public HeroSelectorUI HeroSelector;
    }
}