using System;
using GameCore.Source.Domain.Enums;
using UnityEngine;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public class EnemySpawnData
    {
        [field: SerializeField] public int Count { get; set; }
        [field: SerializeField] public EnemyType EnemyType { get; set; }
    }
}