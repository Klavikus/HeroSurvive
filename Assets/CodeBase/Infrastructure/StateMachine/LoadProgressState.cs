using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.Views;

namespace CodeBase.Infrastructure.StateMachine
{
    public class LoadProgressState : IState
    {
        private const string MainMenuScene = "MainMenu";

        private readonly GameStateMachine _gameStateMachine;

        private IPersistentDataService _persistentDataService;
        private IAdsProvider _adsProvider;
        private ITranslationService _translationService;

        public LoadProgressState(GameStateMachine gameStateMachine) =>
            _gameStateMachine = gameStateMachine;

        public void Enter()
        {
            _persistentDataService = AllServices.Container.AsSingle<IPersistentDataService>();
            _persistentDataService.LoadOrDefaultUpgradeModelsFromLocal();

            _adsProvider = AllServices.Container.AsSingle<IAdsProvider>();
            _adsProvider.Initialized += AdsProviderOnInitialized;
            _adsProvider.Initialize();

            _translationService = AllServices.Container.AsSingle<ITranslationService>();
            _translationService.UpdateLanguage();

            LeaderBoardsViewModel leaderBoardsViewModel =
                AllServices.Container.AsSingle<IViewModelProvider>().LeaderBoardsViewModel;
            leaderBoardsViewModel.Initialize();
        }

        private void AdsProviderOnInitialized()
        {
            _gameStateMachine.Enter<LoadLevelState, string>(MainMenuScene);
        }

        public void Exit()
        {
            _adsProvider.Initialized -= AdsProviderOnInitialized;
        }
    }
}