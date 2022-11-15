using System;
using System.Linq;
using CodeBase.Domain.Data;
using CodeBase.Domain.Enums;
using UnityEngine;

namespace CodeBase.Configs
{
    [CreateAssetMenu(menuName = "Create EnemyConfigSO", fileName = "EnemyConfigSO", order = 0)]
    public class EnemyConfigSO : ScriptableObject
    {
        [field: SerializeField] public EnemyData[] EnemiesData { get; private set; }

        private void OnValidate()
        {
            if (EnemiesData.Select(enemyData => enemyData.Type).Distinct().Count() < EnemiesData.Length)
                throw new ArgumentException($"{nameof(EnemiesData)} should contain only distinct EnemyTypes");

            if (EnemiesData.Select(enemyData => enemyData.Type).Distinct().Count() <
                Enum.GetValues(typeof(EnemyType)).Length)
                throw new ArgumentException($"{nameof(EnemiesData)} should contain all EnemyTypes");
        }
    }
}