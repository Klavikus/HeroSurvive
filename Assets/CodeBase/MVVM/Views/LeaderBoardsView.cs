using System.Collections.Generic;
using Agava.YandexGames;
using CodeBase.Configs;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
using CodeBase.MVVM.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views
{
    public class LeaderBoardsView : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Transform _scoreViewsContainer;
        [SerializeField] private Button _closeButton;
        [SerializeField] private LeaderBoardScoreView _playerScoreView;

        private List<LeaderBoardScoreView> _leaderBoardScoreViews;
        private LeaderBoardsViewModel _leaderBoardsViewModel;
        private MenuViewModel _menuViewModel;
        private IViewFactory _viewFactory;

        private void Start()
        {
            Hide();
            _leaderBoardsViewModel = AllServices.Container.AsSingle<IViewModelProvider>().LeaderBoardsViewModel;
            _menuViewModel = AllServices.Container.AsSingle<IViewModelProvider>().MenuViewModel;
            _viewFactory = AllServices.Container.AsSingle<IViewFactory>();
            _leaderBoardsViewModel.PlayerScoreUpdated += UpdatePlayerScoreView;
            _leaderBoardsViewModel.LeaderBoardUpdated += CreateScoreViews;
            _menuViewModel.ShowLeaderBordInvoked += Show;
            _menuViewModel.HideLeaderBordInvoked += Hide;
            _closeButton.onClick.AddListener(_menuViewModel.InvokeLeaderBoardHide);
            _leaderBoardScoreViews = new List<LeaderBoardScoreView>();
            UpdatePlayerScoreView();
            CreateScoreViews();
        }

        private void UpdatePlayerScoreView()
        {
            LeaderboardEntryResponse entry = _leaderBoardsViewModel.GetPlayerScoreEntry();

            if (entry == null)
                return;

            _playerScoreView.Initialize(entry.extraData, entry.score, entry.rank);
        }

        private void OnDisable()
        {
            _leaderBoardsViewModel.LeaderBoardUpdated -= CreateScoreViews;
            _leaderBoardsViewModel.PlayerScoreUpdated -= UpdatePlayerScoreView;
            _menuViewModel.ShowLeaderBordInvoked -= Show;
            _menuViewModel.HideLeaderBordInvoked -= Hide;
            _closeButton.onClick.RemoveListener(_menuViewModel.InvokeLeaderBoardHide);
        }

        private void CreateScoreViews()
        {
            IReadOnlyCollection<LeaderboardEntryResponse> entries =
                _leaderBoardsViewModel.GetLeaderboardEntries(GameConstants.StageTotalKillsLeaderBoardKey);

            if (entries == null)
                return;

            foreach (LeaderBoardScoreView leaderBoardScoreView in _leaderBoardScoreViews)
                Destroy(leaderBoardScoreView.gameObject);
            _leaderBoardScoreViews.Clear();

            foreach (LeaderboardEntryResponse entryData in entries)
            {
                LeaderBoardScoreView scoreView = _viewFactory.CreateLeaderBoardScoreView();
                scoreView.Initialize(entryData.extraData, entryData.score, entryData.rank);
                scoreView.transform.SetParent(_scoreViewsContainer);
                scoreView.transform.localScale = Vector3.one;
                _leaderBoardScoreViews.Add(scoreView);
            }
        }

        private void Show() => _mainCanvas.enabled = true;
        private void Hide() => _mainCanvas.enabled = false;
    }
}