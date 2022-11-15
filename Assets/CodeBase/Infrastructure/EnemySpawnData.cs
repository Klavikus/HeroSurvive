using System;
using CodeBase.Domain.Enums;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    [Serializable]
    public class EnemySpawnData
    {
        [field: SerializeField] public int Count { get; set; }
        [field: SerializeField] public EnemyType EnemyType { get; set; }
    }
}