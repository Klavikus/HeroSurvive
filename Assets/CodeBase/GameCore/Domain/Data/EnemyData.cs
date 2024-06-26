using System;
using CodeBase.GameCore.Domain.Enemies;
using CodeBase.GameCore.Domain.Enums;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Data
{
    [Serializable]
    public class EnemyData
    {
        public EnemyData(EnemyType type, Enemy prefab)
        {
            Type = type;
            Name = type.ToString();
            Prefab = prefab;
        }

        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField] public Enemy Prefab { get; private set; }
        [field: SerializeField] public EnemyAIData AIData { get; private set; }
        [field: SerializeField] public DamageableData DamageableData { get; private set; }
        [field: SerializeField] public DamageSourceData DamageSourceData { get; private set; }
        [field: SerializeField] public LootData LootData { get; private set; }
        [field: SerializeField] public ProgressionData ProgressionData { get; private set; }

        public void SetProgressionData(ProgressionData progressionData) =>
            ProgressionData = progressionData;

        public void SetData(
            DamageableData damageableData,
            DamageSourceData damageSourceData,
            LootData lootData,
            EnemyAIData enemyAIData,
            ProgressionData baseProgressionData)
        {
            AIData = enemyAIData;
            DamageableData = damageableData;
            DamageSourceData = damageSourceData;
            LootData = lootData;
            ProgressionData = baseProgressionData;
        }
    }
}