using System;
using System.Collections.Generic;
using Agava.YandexGames;
using UnityEngine;

namespace CodeBase.MVVM.Models
{
    public class LeaderBoard
    {
        private readonly UserModel _userModel;
        private readonly Dictionary<string, int> _cachedEntries;
        
        private readonly List<LeaderboardEntryResponse> _cashedEntriesResponse;
 
        public string Name { get; }
        public IReadOnlyDictionary<string, int> Entries => _cachedEntries;

        public event Action<LeaderBoard> Updated;
        public event Action<string, int> ScoreSetted;

        public LeaderBoard(string name, UserModel userModel)
        {
            Name = name;
            _userModel = userModel;
            _cachedEntries = new Dictionary<string, int>();
        }

        public void UpdateOrCreateEntries(LeaderboardGetEntriesResponse entriesResponse)
        {
            Debug.Log("UpdateOrCreateEntries");
            foreach (LeaderboardEntryResponse entry in entriesResponse.entries)
            {
                string name = entry.extraData;

                Debug.Log($"UpdateOrCreateEntries name {name}");

                if (string.IsNullOrEmpty(name))
                    name = "Anonymous";

                if (_cachedEntries.ContainsKey(name))
                    _cachedEntries[name] = entry.score;
                else
                    _cachedEntries.Add(name, entry.score);
            }

            Debug.Log("UpdateOrCreateEntries done");
            Updated?.Invoke(this);
        }
        public void SetScore(int newScore)
        {
            Debug.Log("SetScore...");

            if (_cachedEntries.ContainsKey(_userModel.Name))
                // &&
                // newScore > _cachedEntries[_userModel.Name])
            {
                Debug.Log("Updated");

                ScoreSetted?.Invoke(Name, newScore);
                _cachedEntries[_userModel.Name] = newScore;
            }
            else
            {
                Debug.Log("new score");

                ScoreSetted?.Invoke(Name, newScore);

                _cachedEntries.Add(_userModel.Name, newScore);
            }
        }
        public int GetScore() => _cachedEntries.ContainsKey(_userModel.Name) ? _cachedEntries[_userModel.Name] : 0;
    }
}