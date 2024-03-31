using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Presentation.Api;
using Modules.Common.WindowFsm.Runtime.Abstract;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class LeaderBoardPresenter : BaseWindowPresenter<LeaderBoardWindow>
    {
        private readonly ILeaderBoardsView _view;

        public LeaderBoardPresenter(
            ILeaderBoardsView view,
            IWindowFsm windowFsm)
            : base(windowFsm, view.MainCanvas)
        {
            _view = view;
        }

        protected override void OnAfterEnable()
        {
            _view.CloseButton.Initialize();
        }

        protected override void OnAfterDisable()
        {
        }
    }
}