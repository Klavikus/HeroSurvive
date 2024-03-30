using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Enemies;
using GameCore.Source.Domain.Enums;
using NaughtyAttributes;
using UnityEngine;

namespace GameCore.Source.Domain.Configs
{
    [CreateAssetMenu(menuName = "Create EnemyConfigSO", fileName = "EnemyConfigSO", order = 0)]
    public class EnemyConfigSO : ScriptableObject
    {
        //TODO: Вынести в отдельный монобех для левелдизайна
        [SerializeField] private DamageableData _damageableData;
        [SerializeField] private DamageSourceData _damageSourceData;
        [SerializeField] private LootData _lootData;
        [SerializeField] private EnemyAIData _enemyAIData;
        [SerializeField] private ProgressionData _baseProgressionData;

        [Button(nameof(SetBaseProgressionToAll))]
        public void SetBaseProgressionToAll()
        {
            foreach (EnemyData enemyData in EnemiesData)
                enemyData.SetProgressionData(_baseProgressionData);
        }

        [Button(nameof(SetBaseDataToAll))]
        public void SetBaseDataToAll()
        {
            foreach (EnemyData enemyData in EnemiesData)
                enemyData.SetData(_damageableData, _damageSourceData, _lootData, _enemyAIData, _baseProgressionData);
        }

        [Button(nameof(CreateDefault))]
        public void CreateDefault()
        {
            var result = new List<EnemyData>();
            foreach (EnemyType type in Enum.GetValues(typeof(EnemyType)))
            {
                GameObject prefab = Resources.Load<GameObject>($"Enemies/{type.ToString()}");
                result.Add(new EnemyData(type, prefab));
            }

            EnemiesData = result.ToArray();
        }

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