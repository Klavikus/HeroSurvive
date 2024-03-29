using CodeBase.GameCore.Domain.Data;
using UnityEngine;

namespace CodeBase.GameCore.Infrastructure
{
    [CreateAssetMenu(menuName = "Create StageCompetitionConfigSO", fileName = "StageCompetitionConfigSO", order = 0)]
    public class StageCompetitionConfigSO : ScriptableObject
    {
        [field: SerializeField] public StageData[] WavesData { get; private set; }
    }
}