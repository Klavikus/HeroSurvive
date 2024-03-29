using System;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Data
{
    [Serializable]
    public class StageData
    {
        [field: SerializeField] public EnemySpawnData[] EnemiesSpawnData { get; private set; }
        [field: SerializeField] public int PerStage { get; private set; }
    }
}