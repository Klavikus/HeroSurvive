using GameCore.Source.Presentation.Api.MainMenu;
using Modules.MVPPassiveView.Runtime;
using Modules.UIComponents.Runtime.Implementations.Buttons;
using UnityEngine;

namespace GameCore.Source.Presentation.Core.MainMenu
{
    public class MVPLeaderBoardsView : ViewBase, ILeaderBoardsView
    {
        [SerializeField] private MVPLeaderBoardScoreView _playerLeaderBoardScoreView;
        [field: SerializeField] public Canvas MainCanvas { get; private set; }
        [field: SerializeField] public Transform ScoreViewsContainer { get; private set; }
        [field: SerializeField] public ActionButton CloseButton { get; private set; }

        public ILeaderBoardScoreView PlayerLeaderBoardScoreView => _playerLeaderBoardScoreView;

        // [SerializeField] private Button _closeButton;
        // [SerializeField] private LeaderBoardScoreView _playerScoreView;
        // [SerializeField] private MVPLeaderBoardAuthorizeRequestView _authorizeRequestView;

        // private List<LeaderBoardScoreView> _leaderBoardScoreViews;
        // private LeaderBoardsViewModel _leaderBoardsViewModel;
        // private IViewFactory _viewFactory;
        // private ITranslationService _translationService;

        private void Start()
        {
            return;

            // Hide();
            // _leaderBoardsViewModel = AllServices.Container.AsSingle<IViewModelProvider>().Get<LeaderBoardsViewModel>();
            // _viewFactory = AllServices.Container.AsSingle<IViewFactory>();
            // _translationService = AllServices.Container.AsSingle<ITranslationService>();
            // _playerScoreView.Initialize(_translationService);
            // _authorizeRequestView.Initialize(_leaderBoardsViewModel);
            // _leaderBoardsViewModel.PlayerScoreUpdated += UpdatePlayerScoreView;
            // _leaderBoardsViewModel.LeaderBoardUpdated += CreateScoreViews;
            // _leaderBoardsViewModel.ShowLeaderBordInvoked += Show;
            // _leaderBoardsViewModel.HideLeaderBordInvoked += Hide;
            // _closeButton.onClick.AddListener(_leaderBoardsViewModel.InvokeLeaderBoardHide);
            // _leaderBoardScoreViews = new List<LeaderBoardScoreView>();
            // UpdatePlayerScoreView();
            // CreateScoreViews();
        }

        // private void UpdatePlayerScoreView()
        // {
        //     LeaderboardEntryResponse entry = _leaderBoardsViewModel.GetPlayerScoreEntry();
        //
        //     if (entry == null)
        //         return;
        //
        //     _playerScoreView.Render(entry.player.publicName, entry.score, entry.rank);
        // }
        //
        // private void OnDisable()
        // {
        //     _leaderBoardsViewModel.LeaderBoardUpdated -= CreateScoreViews;
        //     _leaderBoardsViewModel.PlayerScoreUpdated -= UpdatePlayerScoreView;
        //     _leaderBoardsViewModel.ShowLeaderBordInvoked -= Show;
        //     _leaderBoardsViewModel.HideLeaderBordInvoked -= Hide;
        //     _closeButton.onClick.RemoveListener(_leaderBoardsViewModel.InvokeLeaderBoardHide);
        // }
        //
        // private void CreateScoreViews()
        // {
        //     IReadOnlyCollection<LeaderboardEntryResponse> entries =
        //         _leaderBoardsViewModel.GetLeaderboardEntries(GameConstants.StageTotalKillsLeaderBoardKey);
        //
        //     if (entries == null)
        //         return;
        //
        //     foreach (LeaderBoardScoreView leaderBoardScoreView in _leaderBoardScoreViews)
        //         Destroy(leaderBoardScoreView.gameObject);
        //     _leaderBoardScoreViews.Clear();
        //
        //     foreach (LeaderboardEntryResponse entryData in entries)
        //     {
        //         LeaderBoardScoreView scoreView = _viewFactory.CreateLeaderBoardScoreView();
        //         scoreView.Initialize(_translationService);
        //
        //         string userName = entryData.player.publicName;
        //
        //         if (string.IsNullOrEmpty(userName)) 
        //             userName = _translationService.GetLocalizedHiddenUser();
        //
        //         scoreView.Render(userName, entryData.score, entryData.rank);
        //         scoreView.transform.SetParent(_scoreViewsContainer);
        //         scoreView.transform.localScale = Vector3.one;
        //         _leaderBoardScoreViews.Add(scoreView);
        //     }
        // }

        // private void Show() => _mainCanvas.enabled = true;
        // private void Hide() => _mainCanvas.enabled = false;
    }
}