using Agava.YandexGames;
using CodeBase.Configs;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.Views;
using UnityEngine;

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
            _adsProvider = AllServices.Container.AsSingle<IAdsProvider>();
            _translationService = AllServices.Container.AsSingle<ITranslationService>();

            _persistentDataService.LoadOrDefaultUpgradeModels();
            _gameStateMachine.Enter<LoadLevelState, string>(MainMenuScene);

            _adsProvider.Initialized += AdsProviderOnInitialized;
        }

        private void AdsProviderOnInitialized()
        {
            PlayerAccount.Authorize(onSuccessCallback: () =>
            {
                _translationService.UpdateLanguage();
                _persistentDataService.LoadOrDefaultUpgradeModels();
                _gameStateMachine.Enter<LoadLevelState, string>(MainMenuScene);
                LeaderBoardsViewModel leaderBoardsViewModel =
                    AllServices.Container.AsSingle<IViewModelProvider>().LeaderBoardsViewModel;
                leaderBoardsViewModel.UpdateLocalLeaderBoards();
                leaderBoardsViewModel.StartAutoUpdate();
            });
        }

        public void Exit()
        {
        }
    }
}