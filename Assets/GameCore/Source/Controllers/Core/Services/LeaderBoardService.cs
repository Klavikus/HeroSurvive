using System;
using System.Collections;
using System.Collections.Generic;
using Agava.YandexGames;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using GameCore.Source.Infrastructure.Api.Services;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Services
{
    public class LeaderBoardService
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
        public event Action ShowLeaderBordInvoked;
        public event Action HideLeaderBordInvoked;
        public event Action LeaderboardAuthorizeRequest;
        public event Action<bool> LeaderboardAuthorizeRequestHandled;

        public LeaderBoardService(
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

        private void OnLeaderBoardShowInvoked()
        {
            if (_authorizeService.IsAuthorized)
                ShowLeaderBordInvoked?.Invoke();
            else
                LeaderboardAuthorizeRequest?.Invoke();
        }

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
     
        }

        private void StopAutoUpdate()
        {

        }

        private void UpdateLocalLeaderBoards()
        {
// #if UNITY_EDITOR
            Debug.LogWarning("Trying to get leaderboard entries in editor!");

            // return;
// #endif
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