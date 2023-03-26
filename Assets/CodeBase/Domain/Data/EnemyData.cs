using System;
using UnityEngine;

namespace CodeBase.Domain
{
    [Serializable]
    public class EnemyData
    {
        [field: SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField] public Enemy Prefab { get; private set; }
        [field: SerializeField] public EnemyAIData AIData { get; private set; }
        [field: SerializeField] public DamageableData DamageableData { get; private set; }
        [field: SerializeField] public DamageSourceData DamageSourceData { get; private set; }
        [field: SerializeField] public LootData LootData { get; private set; }
        [field: SerializeField] public ProgressionData ProgressionData { get; private set; }

        public void SetProgressionData(ProgressionData progressionData) => ProgressionData = progressionData;
    }
}