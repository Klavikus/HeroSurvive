using System;
using System.Collections.Generic;
using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Infrastructure.Core.Services.DI;
using GameCore.Source.Presentation.Core.MainMenu;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.Common.WindowFsm.Runtime.Implementation;
using UnityEngine;

namespace GameCore.Source.Application.CompositionRoots
{
    public class MainMenuCompositionRoot : SceneCompositionRoot
    {
        [SerializeField] private MVPLeaderBoardsView _leaderBoardsView;

        public override async void Initialize(ServiceContainer serviceContainer)
        {
            Dictionary<Type, IWindow> windows = new Dictionary<Type, IWindow>()
            {
                [typeof(RootWindow)] = new RootWindow(),
                [typeof(LeaderBoardWindow)] = new LeaderBoardWindow(),
            };
            
            WindowFsm<RootWindow> windowFsm = new WindowFsm<RootWindow>(windows);

            LeaderBoardPresenter leaderBoardPresenter = new LeaderBoardPresenter(_leaderBoardsView, windowFsm);
            _leaderBoardsView.Construct(leaderBoardPresenter);
        }
    }
}