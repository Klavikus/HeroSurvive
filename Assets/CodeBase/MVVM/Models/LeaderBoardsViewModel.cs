using System;
using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
using CodeBase.Configs;
using CodeBase.Domain.Additional;
using UnityEngine;

namespace CodeBase.MVVM.Models
{
    public class LeaderBoardsViewModel
    {
        private readonly IAuthorizeService _authorizeService;
        private readonly LeaderBoard[] _leaderBoards;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Dictionary<string, LeaderBoard> _leaderBoardByNames;
        private readonly WaitForSeconds _passiveUpdateDelay = new(GameConstants.LeaderBoardPassiveUpdateDelay);

        private Coroutine _autoUpdateCoroutine;
        private LeaderboardEntryResponse _playerCachedEntry;
        private bool _playerNotInLeaderBoard;
        public event Action LeaderBoardUpdated;
        public event Action PlayerScoreUpdated;

        public LeaderBoardsViewModel(
            IAuthorizeService authorizeService,
            LeaderBoard[] leaderBoards,
            ICoroutineRunner coroutineRunner)
        {
            _authorizeService = authorizeService;
            _leaderBoards = leaderBoards;
            _coroutineRunner = coroutineRunner;
            _leaderBoardByNames = new Dictionary<string, LeaderBoard>();
        }

        public void Initialize()
        {
            foreach (LeaderBoard leaderBoard in _leaderBoards)
            {
                _leaderBoardByNames.Add(leaderBoard.Name, leaderBoard);
                leaderBoard.Updated += OnLocalLeaderBoardUpdated;
            }

            _authorizeService.Authorized += OnAuthorized;
        }

        ~LeaderBoardsViewModel()
        {
            foreach (LeaderBoard leaderBoard in _leaderBoards)
                leaderBoard.Updated -= OnLocalLeaderBoardUpdated;

            _authorizeService.Authorized -= OnAuthorized;
            StopAutoUpdate();
        }

        private void OnAuthorized() => StartAutoUpdate();

        public IReadOnlyCollection<LeaderboardEntryResponse> GetLeaderboardEntries(string leaderBoardName) =>
            _leaderBoardByNames[leaderBoardName].CachedEntries;

        public LeaderboardEntryResponse GetPlayerScoreEntry() => _playerCachedEntry;

        public void SetMaxScore(int currentEnemyKilled)
        {
            if (_playerNotInLeaderBoard)
                SetScore(GameConstants.StageTotalKillsLeaderBoardKey, currentEnemyKilled);

            if (_playerCachedEntry != null && _playerCachedEntry.score < currentEnemyKilled)
                SetScore(GameConstants.StageTotalKillsLeaderBoardKey, currentEnemyKilled);
        }

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
            foreach (LeaderBoard leaderBoard in _leaderBoards)
                Leaderboard.GetEntries(leaderBoard.Name,
                    onSuccessCallback: result => leaderBoard.UpdateOrCreateEntries(result));

            Leaderboard.GetPlayerEntry(GameConstants.StageTotalKillsLeaderBoardKey, OnGetPlayerScoreSuccessCallback);
        }

        private void SetScore(string leaderBoardName, int newScore) =>
            Leaderboard.SetScore(leaderBoardName, newScore);

        private IEnumerator UpdateLeaderBoardCoroutine()
        {
            yield return _passiveUpdateDelay;
            UpdateLocalLeaderBoards();
        }

        private void OnLocalLeaderBoardUpdated(LeaderBoard leaderBoard) => LeaderBoardUpdated?.Invoke();

        private void OnGetPlayerScoreSuccessCallback(LeaderboardEntryResponse response)
        {
            _playerNotInLeaderBoard = response == null;
            _playerCachedEntry = response;
            PlayerScoreUpdated?.Invoke();
        }
    }
}