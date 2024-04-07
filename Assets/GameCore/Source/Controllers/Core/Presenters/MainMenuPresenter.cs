using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api.MainMenu;
using Modules.Common.WindowFsm.Runtime.Abstract;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class MainMenuPresenter : BaseWindowPresenter<MainMenuWindow>
    {
        private readonly IMainMenuView _view;
        private readonly IGameStateMachine _gameStateMachine;

        public MainMenuPresenter(
            IWindowFsm windowFsm,
            IMainMenuView view)
            : base(windowFsm, view.Canvas)
        {
            _view = view;
        }

        protected override void OnAfterEnable()
        {
            _view.StartButton.Initialize();
            _view.LeaderBoardButton.Initialize();
            _view.PersistentUpgradesButton.Initialize();

            _view.StartButton.Clicked += OnStartButtonClicked;
            _view.LeaderBoardButton.Clicked += OnLeaderBoardButtonClicked;
            _view.PersistentUpgradesButton.Clicked += OnPersistentUpgradesButtonClicked;
        }

        protected override void OnAfterDisable()
        {
            _view.StartButton.Clicked -= OnStartButtonClicked;
            _view.LeaderBoardButton.Clicked -= OnLeaderBoardButtonClicked;
            _view.PersistentUpgradesButton.Clicked -= OnPersistentUpgradesButtonClicked;
        }

        private void OnStartButtonClicked() =>
            WindowFsm.OpenWindow<HeroSelectorWindow>();

        private void OnLeaderBoardButtonClicked() =>
            WindowFsm.OpenWindow<LeaderBoardWindow>();

        private void OnPersistentUpgradesButtonClicked() =>
            WindowFsm.OpenWindow<UpgradeSelectorWindow>();
    }
}