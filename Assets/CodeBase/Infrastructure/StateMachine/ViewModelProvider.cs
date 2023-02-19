using CodeBase.MVVM.Models;
using CodeBase.MVVM.ViewModels;
using CodeBase.MVVM.Views;

namespace CodeBase.Infrastructure.StateMachine
{
    public class ViewModelProvider : IViewModelProvider
    {
        public ViewModelProvider(LeaderBoardsViewModel leaderBoardsViewModel, MenuViewModel menuViewModel)
        {
            LeaderBoardsViewModel = leaderBoardsViewModel;
            MenuViewModel = menuViewModel;
        }

        public LeaderBoardsViewModel LeaderBoardsViewModel { get; }
        public MenuViewModel MenuViewModel { get; }
    }
}