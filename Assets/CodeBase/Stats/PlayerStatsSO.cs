using System;
using System.Collections.Generic;
using CodeBase.Enums;
using UnityEngine;

namespace CodeBase.Stats
{
    [CreateAssetMenu(menuName = "SO/CreateAbilityData/PlayerStatsSO", fileName = "PlayerStatsSO", order = 0)]
    public class PlayerStatsSO : ScriptableObject
    {
        [field: SerializeField] public int Count { get; private set; }

        private Dictionary<PlayerStat, float> _stats;

        public IReadOnlyDictionary<PlayerStat, float> GetStats()
        {
            Dictionary<PlayerStat, float> stats = new Dictionary<PlayerStat, float>();
            foreach (string name in Enum.GetNames(typeof(PlayerStat)))
            {
                Enum.TryParse(name, out PlayerStat statType);
                stats.Add(statType, 0);
            }

            stats[PlayerStat.ProjectilesCountModifier] = Count;

            return stats;
        }
    }
}