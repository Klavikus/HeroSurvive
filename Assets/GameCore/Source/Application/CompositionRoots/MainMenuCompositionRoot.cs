using System;
using System.Collections.Generic;
using GameCore.Source.Application.GameFSM;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Infrastructure.Core.Services.DI;
using GameCore.Source.Presentation.Core.MainMenu;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.Common.WindowFsm.Runtime.Implementation;
using UnityEngine;

namespace GameCore.Source.Application.CompositionRoots
{
    public class MainMenuCompositionRoot : SceneCompositionRoot
    {
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private MVPLeaderBoardsView _leaderBoardsView;

        public override async void Initialize(ServiceContainer serviceContainer)
        {
            Dictionary<Type, IWindow> windows = new Dictionary<Type, IWindow>()
            {
                [typeof(MainMenuWindow)] = new MainMenuWindow(),
                [typeof(LeaderBoardWindow)] = new LeaderBoardWindow(),
            };

            WindowFsm<MainMenuWindow> windowFsm = new WindowFsm<MainMenuWindow>(windows);

            IGameStateMachine gameStateMachine = serviceContainer.Single<IGameStateMachine>();

            MainMenuPresenter mainMenuPresenter = new MainMenuPresenter(windowFsm, _mainMenuView, gameStateMachine);
            _mainMenuView.Construct(mainMenuPresenter);

            LeaderBoardPresenter leaderBoardPresenter = new LeaderBoardPresenter(_leaderBoardsView, windowFsm);
            _leaderBoardsView.Construct(leaderBoardPresenter);
        }
    }
}