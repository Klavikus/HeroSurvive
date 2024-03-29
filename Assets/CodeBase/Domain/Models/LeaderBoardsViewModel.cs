using System;
using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
using CodeBase.Configs;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Domain.Models
{
    public class LeaderBoardsViewModel
    {
        private readonly IAuthorizeService _authorizeService;
        private readonly LeaderBoard[] _leaderBoards;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly MenuModel _menuModel;
        private readonly Dictionary<string, LeaderBoard> _leaderBoardByNames;
        private readonly WaitForSeconds _passiveUpdateDelay = new(GameConstants.LeaderBoardPassiveUpdateDelay);

        private Coroutine _autoUpdateCoroutine;
        private LeaderboardEntryResponse _playerCachedEntry;
        private bool _playerNotInLeaderBoard;
        public event Action LeaderBoardUpdated;
        public event Action PlayerScoreUpdated;
        public event Action ShowLeaderBordInvoked;
        public event Action HideLeaderBordInvoked;
        public event Action LeaderboardAuthorizeRequest;
        public event Action<bool> LeaderboardAuthorizeRequestHandled;

        public LeaderBoardsViewModel(
            IAuthorizeService authorizeService,
            LeaderBoard[] leaderBoards,
            ICoroutineRunner coroutineRunner,
            MenuModel menuModel)
        {
            _authorizeService = authorizeService;
            _leaderBoards = leaderBoards;
            _coroutineRunner = coroutineRunner;
            _menuModel = menuModel;
            _leaderBoardByNames = new Dictionary<string, LeaderBoard>();
        }

        public void Initialize()
        {
            _menuModel.LeaderBoardShowInvoked += OnLeaderBoardShowInvoked;
            _menuModel.LeaderBoardHideInvoked += OnLeaderBoardHideInvoked;

            foreach (LeaderBoard leaderBoard in _leaderBoards)
            {
                _leaderBoardByNames.Add(leaderBoard.Name, leaderBoard);
                leaderBoard.Updated += OnLocalLeaderBoardUpdated;
            }

            _authorizeService.Authorized += OnAuthorized;
        }

        ~LeaderBoardsViewModel()
        {
            _menuModel.LeaderBoardShowInvoked -= OnLeaderBoardShowInvoked;
            _menuModel.LeaderBoardHideInvoked -= OnLeaderBoardHideInvoked;

            foreach (LeaderBoard leaderBoard in _leaderBoards)
                leaderBoard.Updated -= OnLocalLeaderBoardUpdated;

            _authorizeService.Authorized -= OnAuthorized;
            StopAutoUpdate();
        }

        private void OnLeaderBoardShowInvoked()
        {
            if (_authorizeService.IsAuthorized)
                ShowLeaderBordInvoked?.Invoke();
            else
                LeaderboardAuthorizeRequest?.Invoke();
        }

        public void InvokeLeaderBoardHide() => _menuModel.InvokeLeaderBoardHide();

        public IReadOnlyCollection<LeaderboardEntryResponse> GetLeaderboardEntries(string leaderBoardName) =>
            _leaderBoardByNames[leaderBoardName].CachedEntries;

        public LeaderboardEntryResponse GetPlayerScoreEntry() => _playerCachedEntry;

        public void SetMaxScore(int currentEnemyKilled)
        {
            if (_authorizeService.IsAuthorized == false)
                return;

            if (_playerNotInLeaderBoard)
                SetScore(GameConstants.StageTotalKillsLeaderBoardKey, currentEnemyKilled);

            if (_playerCachedEntry != null && _playerCachedEntry.score < currentEnemyKilled)
                SetScore(GameConstants.StageTotalKillsLeaderBoardKey, currentEnemyKilled);
        }

        public void ApproveAuthorizeRequest()
        {
            _authorizeService.Authorize();
            LeaderboardAuthorizeRequestHandled?.Invoke(true);
        }

        public void DeclineAuthorizeRequest() => LeaderboardAuthorizeRequestHandled?.Invoke(false);

        private void OnLeaderBoardHideInvoked() => HideLeaderBordInvoked?.Invoke();

        private void OnLocalLeaderBoardUpdated(LeaderBoard leaderBoard) => LeaderBoardUpdated?.Invoke();

        private void OnAuthorized() => StartAutoUpdate();

        private void StartAutoUpdate()
        {
            if (_autoUpdateCoroutine != null)
                _coroutineRunner.Stop(_autoUpdateCoroutine);

            _autoUpdateCoroutine = _coroutineRunner.Run(UpdateLeaderBoardCoroutine());
        }

        private void StopAutoUpdate()
        {
            if (_autoUpdateCoroutine != null)
                _coroutineRunner.Stop(_autoUpdateCoroutine);
        }

        private void UpdateLocalLeaderBoards()
        {
#if UNITY_EDITOR
            Debug.LogWarning("Trying to get leaderboard entries in editor!");
            return;
#endif
            foreach (LeaderBoard leaderBoard in _leaderBoards)
                Leaderboard.GetEntries(leaderBoard.Name,
                    onSuccessCallback: result => leaderBoard.UpdateOrCreateEntries(result));

            Leaderboard.GetPlayerEntry(GameConstants.StageTotalKillsLeaderBoardKey, OnGetPlayerScoreSuccessCallback);
        }

        private void SetScore(string leaderBoardName, int newScore) =>
            Leaderboard.SetScore(leaderBoardName, newScore);

        private IEnumerator UpdateLeaderBoardCoroutine()
        {
            UpdateLocalLeaderBoards();
            yield return _passiveUpdateDelay;
        }

        private void OnGetPlayerScoreSuccessCallback(LeaderboardEntryResponse response)
        {
            _playerNotInLeaderBoard = response == null;
            _playerCachedEntry = response;
            PlayerScoreUpdated?.Invoke();
        }
    }
}