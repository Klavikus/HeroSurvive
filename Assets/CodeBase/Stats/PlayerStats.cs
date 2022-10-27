using System;
using System.Collections.Generic;
using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Stats
{
    public class PlayerStats
    {
        public int ProjectilesCountModifier { get; private set; }
        public float DamageModifier { get; private set; }
        public float SpeedModifier { get; private set; }
        public float SizeModifier { get; private set; }
        public float DurationModifier { get; private set; }
        public float CooldownModifier { get; private set; }

        public IReadOnlyDictionary<PlayerStat, float> GetStats()
        {
            Dictionary<PlayerStat, float> stats = new Dictionary<PlayerStat, float>();
            foreach (string name in Enum.GetNames(typeof(PlayerStat)))
            {
                Enum.TryParse(name, out PlayerStat statType);
                stats.Add(statType, 0);
            }

            stats[PlayerStat.ProjectilesCountModifier] = ProjectilesCountModifier;

            return stats;
        }
    }

    public class PlayerStatsHandler
    {
        private readonly Dictionary<PlayerStat, float> _baseStats;
        private readonly Dictionary<PlayerStat, float> _resultStats;

        public PlayerStatsHandler(IReadOnlyDictionary<PlayerStat, float> baseStats)
        {
            _baseStats = CreateStatsDictionary(baseStats.Keys);
            _resultStats = CreateStatsDictionary(baseStats.Keys);
            foreach (PlayerStat playerStat in baseStats.Keys)
                Debug.Log($"{playerStat}");
            FillStatsDictionary(_baseStats, baseStats);
            FillStatsDictionary(_resultStats, baseStats);
        }

        public IReadOnlyDictionary<PlayerStat, float> GetStats() => _resultStats;

        public void UpgradeStats(IReadOnlyDictionary<PlayerStat, float> modifiedStats)
        {
            foreach (PlayerStat key in modifiedStats.Keys)
                _resultStats[key] = _baseStats[key] + modifiedStats[key];
        }

        private Dictionary<PlayerStat, float> CreateStatsDictionary(IEnumerable<PlayerStat> statTypes,
            float baseValue = 0f)
        {
            Dictionary<PlayerStat, float> statsDictionary = new Dictionary<PlayerStat, float>();
            foreach (PlayerStat statType in statTypes)
                statsDictionary.Add(statType, baseValue);
            return statsDictionary;
        }

        private void FillStatsDictionary(Dictionary<PlayerStat, float> dictionary,
            IReadOnlyDictionary<PlayerStat, float> stats)
        {
            foreach (PlayerStat playerStat in stats.Keys)
                dictionary[playerStat] = stats[playerStat];
        }
    }
}