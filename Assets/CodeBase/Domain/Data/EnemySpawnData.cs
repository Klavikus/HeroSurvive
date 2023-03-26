using System;
using UnityEngine;

namespace CodeBase.Domain
{
    [Serializable]
    public class EnemySpawnData
    {
        [field: SerializeField] public int Count { get; set; }
        [field: SerializeField] public EnemyType EnemyType { get; set; }
    }
}