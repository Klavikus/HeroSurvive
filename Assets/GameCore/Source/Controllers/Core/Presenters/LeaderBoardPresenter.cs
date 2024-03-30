using System;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Presentation.Api;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class LeaderBoardPresenter : IPresenter
    {
        private readonly ILeaderBoardsView _view;
        private readonly IWindowFsm _windowFsm;

        public LeaderBoardPresenter(
            ILeaderBoardsView leaderBoardsView,
            IWindowFsm windowFsm)
        {
            _view = leaderBoardsView ?? throw new ArgumentNullException(nameof(leaderBoardsView));
            _windowFsm = windowFsm ?? throw new ArgumentNullException(nameof(windowFsm));
        }

        public void Enable()
        {
            _view.CloseButton.Initialize();

            _windowFsm.Opened += OnWindowOpened;
            _windowFsm.Closed += OnWindowClosed;
        }

        public void Disable()
        {
            _windowFsm.Opened -= OnWindowOpened;
            _windowFsm.Closed -= OnWindowClosed;
        }

        private void OnWindowOpened(IWindow window)
        {
            if (window is not LeaderBoardWindow)
                return;

            _view.MainCanvas.enabled = true;
        }

        private void OnWindowClosed(IWindow window)
        {
            if (window is not LeaderBoardWindow)
                return;

            _view.MainCanvas.enabled = false;
        }
    }
}