using CodeBase.MVVM.Models;
using CodeBase.MVVM.ViewModels;
using CodeBase.MVVM.Views;

namespace CodeBase.Infrastructure.StateMachine
{
    public class ViewModelProvider : IViewModelProvider
    {
        public ViewModelProvider(UserNameViewModel userNameViewModel, 
            LeaderBoardsViewModel leaderBoardsViewModel, MenuViewModel menuViewModel)
        {
            UserNameViewModel = userNameViewModel;
            LeaderBoardsViewModel = leaderBoardsViewModel;
            MenuViewModel = menuViewModel;
        }

        public UserNameViewModel UserNameViewModel { get; }
        public LeaderBoardsViewModel LeaderBoardsViewModel { get; }
        public MenuViewModel MenuViewModel { get; }
    }
}