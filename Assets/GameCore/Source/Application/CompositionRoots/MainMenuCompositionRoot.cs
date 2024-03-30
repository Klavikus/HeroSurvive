using GameCore.Source.Controllers.Core.Presenters;
using GameCore.Source.Infrastructure.Core.Services.DI;
using GameCore.Source.Presentation.Core.MainMenu;
using UnityEngine;

namespace GameCore.Source.Application.CompositionRoots
{
    public class MainMenuCompositionRoot : SceneCompositionRoot
    {
        [SerializeField] private MVPLeaderBoardsView _leaderBoardsView;

        public override async void Initialize(ServiceContainer serviceContainer)
        {
            LeaderBoardPresenter leaderBoardPresenter = new LeaderBoardPresenter();
            _leaderBoardsView.Construct(leaderBoardPresenter);
        }
    }
}