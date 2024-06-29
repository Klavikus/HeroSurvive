using GameCore.Source.Domain.Data;
using UnityEngine;

namespace GameCore.Source.Domain.Configs
{
    [CreateAssetMenu(menuName = "Create AudioConfigSo", fileName = "AudioConfigSo", order = 0)]
    public class AudioConfigSo : ScriptableObject
    {
        [field: SerializeField] public AudioConfig AudioConfig { get; private set; }
    }
}