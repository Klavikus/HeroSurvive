using System;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api;
using Modules.Common.WindowFsm.Runtime.Abstract;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class MainMenuPresenter : BaseWindowPresenter<MainMenuWindow>
    {
        private readonly IMainMenuView _mainMenuView;
        private readonly IGameStateMachine _gameStateMachine;

        public MainMenuPresenter(
            IWindowFsm windowFsm,
            IMainMenuView mainMenuView,
            IGameStateMachine gameStateMachine)
            : base(windowFsm, mainMenuView.Canvas)
        {
            _mainMenuView = mainMenuView;
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
        }

        protected override void OnAfterEnable()
        {
            _mainMenuView.StartButton.Initialize();
            _mainMenuView.StartButton.Clicked += _gameStateMachine.GoToGameLoop;
        }

        protected override void OnAfterDisable()
        {
            _mainMenuView.StartButton.Clicked -= _gameStateMachine.GoToGameLoop;
        }
    }
}