using CodeBase.MVVM.Models;
using CodeBase.MVVM.ViewModels;
using CodeBase.MVVM.Views;

namespace CodeBase.Infrastructure.StateMachine
{
    public class ViewModelProvider : IViewModelProvider
    {
        public ViewModelProvider(UserNameViewModel userNameViewModel, 
            LeaderBoardsViewModel leaderBoardsViewModel)
        {
            UserNameViewModel = userNameViewModel;
            LeaderBoardsViewModel = leaderBoardsViewModel;
        }

        public UserNameViewModel UserNameViewModel { get; }
        public LeaderBoardsViewModel LeaderBoardsViewModel { get; }
    }
}