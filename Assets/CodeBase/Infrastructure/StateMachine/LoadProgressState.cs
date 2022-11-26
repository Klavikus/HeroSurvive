using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure.StateMachine
{
    public class LoadProgressState : IState
    {
        private const string MainMenuScene = "MainMenu";

        private readonly GameStateMachine _gameStateMachine;

        private IPersistentDataService _persistentDataService;
        private IAdsProvider _adsProvider;

        public LoadProgressState(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        public void Enter()
        {
            _persistentDataService = AllServices.Container.Single<IPersistentDataService>();
            _adsProvider = AllServices.Container.Single<IAdsProvider>();
           
            _persistentDataService.LoadOrDefaultUpgradeModels();
            _gameStateMachine.Enter<LoadLevelState, string>(MainMenuScene);
            
            // _adsProvider.Initialized += AdsProviderOnInitialized;
        }

        private void AdsProviderOnInitialized()
        {
            _persistentDataService.LoadOrDefaultUpgradeModels();
            _gameStateMachine.Enter<LoadLevelState, string>(MainMenuScene);
        }

        public void Exit()
        {
        }
    }
}