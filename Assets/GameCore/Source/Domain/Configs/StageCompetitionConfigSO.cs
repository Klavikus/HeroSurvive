using GameCore.Source.Domain.Data;
using UnityEngine;

namespace GameCore.Source.Domain.Configs
{
    [CreateAssetMenu(menuName = "Create StageCompetitionConfigSO", fileName = "StageCompetitionConfigSO", order = 0)]
    public class StageCompetitionConfigSO : ScriptableObject
    {
        [field: SerializeField] public StageData[] WavesData { get; private set; }
    }
}