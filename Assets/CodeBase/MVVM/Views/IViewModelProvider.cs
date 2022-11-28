using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.ViewModels;

namespace CodeBase.MVVM.Views
{
    public interface IViewModelProvider : IService
    {
        public UserNameViewModel UserNameViewModel { get; }
        public LeaderBoardsViewModel LeaderBoardsViewModel { get; }
        public MenuViewModel MenuViewModel { get; }
    }
}