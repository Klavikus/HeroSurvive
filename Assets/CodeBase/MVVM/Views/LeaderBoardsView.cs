using System;
using System.Collections.Generic;
using Agava.YandexGames;
using CodeBase.Configs;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
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
        [SerializeField] private LeaderBoardAuthorizeRequestView _authorizeRequestView;

        private List<LeaderBoardScoreView> _leaderBoardScoreViews;
        private LeaderBoardsViewModel _leaderBoardsViewModel;
        private IViewFactory _viewFactory;
        private ITranslationService _translationService;

        private void Start()
        {
            Hide();
            _leaderBoardsViewModel = AllServices.Container.AsSingle<IViewModelProvider>().LeaderBoardsViewModel;
            _viewFactory = AllServices.Container.AsSingle<IViewFactory>();
            _translationService = AllServices.Container.AsSingle<ITranslationService>();
            _playerScoreView.Initialize(_translationService);
            _authorizeRequestView.Initialize(_leaderBoardsViewModel);
            _leaderBoardsViewModel.PlayerScoreUpdated += UpdatePlayerScoreView;
            _leaderBoardsViewModel.LeaderBoardUpdated += CreateScoreViews;
            _leaderBoardsViewModel.ShowLeaderBordInvoked += Show;
            _leaderBoardsViewModel.HideLeaderBordInvoked += Hide;
            _closeButton.onClick.AddListener(_leaderBoardsViewModel.InvokeLeaderBoardHide);
            _leaderBoardScoreViews = new List<LeaderBoardScoreView>();
            UpdatePlayerScoreView();
            CreateScoreViews();
        }

        private void UpdatePlayerScoreView()
        {
            LeaderboardEntryResponse entry = _leaderBoardsViewModel.GetPlayerScoreEntry();

            if (entry == null)
                return;

            _playerScoreView.Render(entry.player.publicName, entry.score, entry.rank);
        }

        private void OnDisable()
        {
            _leaderBoardsViewModel.LeaderBoardUpdated -= CreateScoreViews;
            _leaderBoardsViewModel.PlayerScoreUpdated -= UpdatePlayerScoreView;
            _leaderBoardsViewModel.ShowLeaderBordInvoked -= Show;
            _leaderBoardsViewModel.HideLeaderBordInvoked -= Hide;
            _closeButton.onClick.RemoveListener(_leaderBoardsViewModel.InvokeLeaderBoardHide);
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
                scoreView.Initialize(_translationService);

                string userName = entryData.player.publicName;

                if (string.IsNullOrEmpty(userName)) 
                    userName = _translationService.GetLocalizedHiddenUser();

                scoreView.Render(userName, entryData.score, entryData.rank);
                scoreView.transform.SetParent(_scoreViewsContainer);
                scoreView.transform.localScale = Vector3.one;
                _leaderBoardScoreViews.Add(scoreView);
            }
        }

        private void Show() => _mainCanvas.enabled = true;
        private void Hide() => _mainCanvas.enabled = false;
    }
}