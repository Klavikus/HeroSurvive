using System;
using System.Collections.Generic;
using Agava.YandexGames;

namespace CodeBase.MVVM.Models
{
    public class LeaderBoard
    {
        private LeaderboardEntryResponse[] _cachedEntriesResponse;
        public string Name { get; }
        public IReadOnlyCollection<LeaderboardEntryResponse> CachedEntries => _cachedEntriesResponse;

        public event Action<LeaderBoard> Updated;

        public LeaderBoard(string name)
        {
            _cachedEntriesResponse = new LeaderboardEntryResponse[] { };
            Name = name;
        }

        public void UpdateOrCreateEntries(LeaderboardGetEntriesResponse entriesResponse)
        {
            _cachedEntriesResponse = entriesResponse.entries;
            Updated?.Invoke(this);
        }
    }
}