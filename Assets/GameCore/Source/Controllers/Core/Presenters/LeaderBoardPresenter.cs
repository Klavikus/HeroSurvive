using System;
using System.Collections.Generic;
using GameCore.Source.Presentation.Core.MainMenu;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.MVPPassiveView.Runtime;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class LeaderBoardPresenter : IPresenter
    {
        private readonly ILeaderBoardsView _leaderBoardsView;
        private readonly IWindowFsm _windowFsm;

        public LeaderBoardPresenter(ILeaderBoardsView leaderBoardsView, IWindowFsm windowFsm)
        {
            _leaderBoardsView = leaderBoardsView;
            _windowFsm = windowFsm;

            throw new NotImplementedException();
        }

        public void Enable()
        {
        }

        public void Disable()
        {
        }
    }
}