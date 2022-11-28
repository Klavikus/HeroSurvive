using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Agava.YandexGames;
using CodeBase.Configs;
using CodeBase.ForSort;
using UnityEngine;

namespace CodeBase.MVVM.Models
{
    public class LeaderBoardsViewModel
    {
        private readonly LeaderBoard[] _leaderBoards;
        private readonly UserModel _userModel;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly Dictionary<string, LeaderBoard> _leaderBoardByNames;
        private readonly WaitForSeconds _passiveUpdateDelay = new(GameConstants.LeaderBoardPassiveUpdateDelay);

        private Coroutine _autoUpdateCoroutine;
        private LeaderboardEntryResponse _playerCachedEntry;
        private bool _playerNotInLeaderBoard;
        public event Action LeaderBoardUpdated;
        public event Action PlayerScoreUpdated;

        public LeaderBoardsViewModel(LeaderBoard[] leaderBoards, UserModel userModel, ICoroutineRunner coroutineRunner)
        {
            _leaderBoards = leaderBoards;
            _userModel = userModel;
            _coroutineRunner = coroutineRunner;
            _leaderBoardByNames = new Dictionary<string, LeaderBoard>();
            foreach (LeaderBoard leaderBoard in _leaderBoards)
            {
                _leaderBoardByNames.Add(leaderBoard.Name, leaderBoard);
                leaderBoard.Updated += OnLocalLeaderBoardUpdated;
            }
        }

        public IReadOnlyCollection<LeaderboardEntryResponse> GetLeaderboardEntries(string leaderBoardName)
        {
            Debug.Log("GetLeaderboardEntries");
            return _leaderBoardByNames[leaderBoardName].CachedEntries;
        }

        public void StartAutoUpdate()
        {
            if (_autoUpdateCoroutine != null)
                _coroutineRunner.Stop(_autoUpdateCoroutine);

            _autoUpdateCoroutine = _coroutineRunner.Run(UpdateLeaderBoardCoroutine());
        }

        public void StopAutoUpdate()
        {
            if (_autoUpdateCoroutine != null)
                _coroutineRunner.Stop(_autoUpdateCoroutine);
        }

        public void SetScore(string leaderBoardName, int newScore) =>
            Leaderboard.SetScore(leaderBoardName, newScore, OnSuccessSetScoreCallback, extraData: _userModel.Name);

        private void OnSuccessSetScoreCallback()
        {
            Debug.Log("OnSuccessSetScoreCallback..");

            UpdateLocalLeaderBoards();
            Debug.Log("OnSuccessSetScoreCallback..done");
        }

        public void UpdateLocalLeaderBoards()
        {
            Debug.Log("UpdateLocalLeaderBoards..");

            foreach (LeaderBoard leaderBoard in _leaderBoards)
                Leaderboard.GetEntries(leaderBoard.Name,
                    onSuccessCallback: result => leaderBoard.UpdateOrCreateEntries(result));

            Leaderboard.GetPlayerEntry(GameConstants.StageTotalKillsLeaderBoardKey, OnGetPlayerScoreSuccessCallback);

            // Leaderboard.GetEntries(leaderBoard.Name, _ => { });
            Debug.Log("UpdateLocalLeaderBoards..done");
        }

        public LeaderboardEntryResponse GetPlayerScoreEntry() => _playerCachedEntry;

        public void SetMaxScore(int currentEnemyKilled)
        {
            if (_playerNotInLeaderBoard) 
                SetScore(GameConstants.StageTotalKillsLeaderBoardKey, currentEnemyKilled);
            
            if (_playerCachedEntry != null && _playerCachedEntry.score < currentEnemyKilled)
                SetScore(GameConstants.StageTotalKillsLeaderBoardKey, currentEnemyKilled);
        }

        private void OnLocalLeaderBoardUpdated(LeaderBoard leaderBoard)
        {
            Debug.Log("OnLocalLeaderBoardUpdated..");
            LeaderBoardUpdated?.Invoke();
        }

        private IEnumerator UpdateLeaderBoardCoroutine()
        {
            yield return _passiveUpdateDelay;
            UpdateLocalLeaderBoards();
        }

        private void OnGetPlayerScoreSuccessCallback(LeaderboardEntryResponse response)
        {
            _playerNotInLeaderBoard = response == null;
            _playerCachedEntry = response;
            PlayerScoreUpdated?.Invoke();
        }
    }
}