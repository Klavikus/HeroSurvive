using System;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api;
using Modules.Common.WindowFsm.Runtime.Abstract;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class MainMenuPresenter : BaseWindowPresenter<MainMenuWindow>
    {
        private readonly IMainMenuView _view;
        private readonly IGameStateMachine _gameStateMachine;

        public MainMenuPresenter(
            IWindowFsm windowFsm,
            IMainMenuView view,
            IGameStateMachine gameStateMachine)
            : base(windowFsm, view.Canvas)
        {
            _view = view;
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
        }

        protected override void OnAfterEnable()
        {
            _view.StartButton.Initialize();
            _view.LeaderBoardButton.Initialize();

            _view.StartButton.Clicked += _gameStateMachine.GoToGameLoop;
            _view.LeaderBoardButton.Clicked += () => WindowFsm.OpenWindow<LeaderBoardWindow>();
        }

        protected override void OnAfterDisable()
        {
            _view.StartButton.Clicked -= _gameStateMachine.GoToGameLoop;
        }
    }
}