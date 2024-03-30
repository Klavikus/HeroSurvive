using UnityEngine;

namespace GameCore.Source.Domain.Configs
{
    [CreateAssetMenu(menuName = "Create GameLoopConfigSO", fileName = "GameLoopConfigSO", order = 0)]
    public class GameLoopConfigSO : ScriptableObject
    {
        [field: SerializeField] public GameObject LevelMapPrefab { get; private set; }
        [field: SerializeField] public GameObject GameLoopView { get; private set; }
    }
}