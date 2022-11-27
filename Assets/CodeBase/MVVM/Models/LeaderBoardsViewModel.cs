using System;
using System.Collections.Generic;
using Agava.YandexGames;
using UnityEngine;

namespace CodeBase.MVVM.Models
{
    public class LeaderBoardsViewModel
    {
        private readonly LeaderBoard[] _leaderBoards;
        private List<LeaderBoardEntryData> _entriesData;
        private UserModel _userModel;

        public IReadOnlyList<LeaderBoardEntryData> EntriesData => _entriesData;

        public event Action<IReadOnlyList<LeaderBoardEntryData>> LeaderBoardUpdated;

        public LeaderBoardsViewModel(LeaderBoard[] leaderBoards, UserModel userModel)
        {
            _leaderBoards = leaderBoards;
            _userModel = userModel;
            foreach (LeaderBoard leaderBoard in _leaderBoards)
            {
                leaderBoard.Updated += OnLeaderBoardUpdated;
                leaderBoard.ScoreSetted += OnLeaderBoardScoreSetted;
            }

            CalculateEntriesData();
        }

        public IReadOnlyList<LeaderBoard> LeaderBoards => _leaderBoards;

        private void OnLeaderBoardScoreSetted(string leaderboardName, int newScore) =>
            Leaderboard.SetScore(leaderboardName, newScore, extraData: _userModel.Name);

        private void OnLeaderBoardUpdated(LeaderBoard leaderBoard)
        {
            CalculateEntriesData();
            LeaderBoardUpdated?.Invoke(_entriesData);
        }

        public void UpdateAll()
        {
            Debug.Log($"{nameof(UpdateAll)}");
            foreach (LeaderBoard leaderBoard in _leaderBoards)
            {
                Debug.Log($"{leaderBoard.Name} UpdateOrCreateEntries");
                Leaderboard.GetEntries(leaderBoard.Name,
                    onSuccessCallback: result => leaderBoard.UpdateOrCreateEntries(result));
            }
        }

        public void SetScore(int range, string leaderBoardName)
        {
            Debug.Log($"SetScore");

            foreach (LeaderBoard leaderBoard in _leaderBoards)
            {
                if (leaderBoard.Name == leaderBoardName)
                    leaderBoard.SetScore(range);
            }
        }

        private void CalculateEntriesData()
        {
            Debug.Log($"CalculateEntriesData 111");
            _entriesData = new List<LeaderBoardEntryData>();
            Debug.Log($"{_leaderBoards.Length}");
            foreach (LeaderBoard leaderBoard in _leaderBoards)
            {
                Debug.Log($"{leaderBoard.Name}");
                Debug.Log($"{leaderBoard.Entries.Count}");
                _entriesData.Add(new LeaderBoardEntryData(leaderBoard.Name, leaderBoard.Entries));
            }

            Debug.Log($"CalculateEntriesData done");
        }
    }
}