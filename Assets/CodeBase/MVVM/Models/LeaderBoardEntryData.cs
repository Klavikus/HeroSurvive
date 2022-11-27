using System.Collections.Generic;

namespace CodeBase.MVVM.Models
{
    public class LeaderBoardEntryData
    {
        public readonly string LeaderBoardName;

        private readonly Dictionary<string, int> _entriesData;

        public IReadOnlyDictionary<string, int> EntriesData => _entriesData;

        public LeaderBoardEntryData(string leaderBoardName, IReadOnlyDictionary<string, int> playersData)
        {
            LeaderBoardName = leaderBoardName;
            _entriesData = new Dictionary<string, int>();
            foreach (KeyValuePair<string, int> keyValuePair in playersData)
                _entriesData.Add(keyValuePair.Key, keyValuePair.Value);
        }
    }
}