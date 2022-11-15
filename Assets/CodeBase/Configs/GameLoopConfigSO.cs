using Cinemachine;
using CodeBase.Infrastructure.Factories;
using CodeBase.MVVM.Views;
using UnityEngine;

namespace CodeBase.Configs
{
    [CreateAssetMenu(menuName = "Create GameLoopConfigSO", fileName = "GameLoopConfigSO", order = 0)]
    public class GameLoopConfigSO : ScriptableObject
    {
        [field: SerializeField] public CinemachineVirtualCamera CameraCM { get; private set; }
        [field: SerializeField] public GameObject LevelMapPrefab { get; private set; }
        [field: SerializeField] public GameLoopView GameLoopView { get; private set; }
    }
}