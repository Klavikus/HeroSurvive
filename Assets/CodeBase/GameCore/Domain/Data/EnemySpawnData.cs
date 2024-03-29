using System;
using CodeBase.GameCore.Domain.Enums;
using UnityEngine;

namespace CodeBase.GameCore.Domain.Data
{
    [Serializable]
    public class EnemySpawnData
    {
        [field: SerializeField] public int Count { get; set; }
        [field: SerializeField] public EnemyType EnemyType { get; set; }
    }
}