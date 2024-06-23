using System;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Presenters.Base;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.GamePauseSystem.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters.MainMenu
{
    public class PausePresenter : BaseWindowPresenter<PauseWindow>
    {
        private readonly IWindowFsm _windowFsm;
        private readonly IPauseView _view;
        private readonly ILocalizationService _localizationService;

        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGamePauseService _gamePauseService;

        public PausePresenter(
            IWindowFsm windowFsm,
            IPauseView view,
            IGameStateMachine gameStateMachine,
            IGamePauseService gamePauseService
        ) : base(windowFsm, view.Show, view.Hide)
        {
            _windowFsm = windowFsm;
            _view = view;
            _gameStateMachine = gameStateMachine ?? throw new ArgumentNullException(nameof(gameStateMachine));
            _gamePauseService = gamePauseService ?? throw new ArgumentNullException(nameof(gamePauseService));
        }

        protected override void OnAfterEnable()
        {
            _view.Initialize();

            _view.CloseButton.Clicked += Resume;
            _view.ResumeButton.Clicked += Resume;
            _view.SettingsButton.Clicked += OnSettingsButtonClicked;
            _view.ExitLevelButton.Clicked += OnExitLevelButtonClicked;
        }

        protected override void OnAfterDisable()
        {
            _view.CloseButton.Clicked -= Resume;
            _view.ResumeButton.Clicked -= Resume;
            _view.SettingsButton.Clicked -= OnSettingsButtonClicked;
            _view.ExitLevelButton.Clicked -= OnExitLevelButtonClicked;
        }

        protected override void OnAfterOpened() =>
            _gamePauseService.InvokeByUI(true);

        protected override void OnAfterClosed() =>
            _gamePauseService.InvokeByUI(false);

        private void Resume() =>
            WindowFsm.Close<PauseWindow>();

        private void OnSettingsButtonClicked() =>
            WindowFsm.OpenWindow<SettingsWindow>();

        private void OnExitLevelButtonClicked() =>
            _gameStateMachine.GoToMainMenu();
    }
}