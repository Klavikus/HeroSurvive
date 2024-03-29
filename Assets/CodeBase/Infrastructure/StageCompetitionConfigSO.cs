using CodeBase.Domain;
using CodeBase.Domain.Data;
using UnityEngine;

namespace CodeBase.Infrastructure
{
    [CreateAssetMenu(menuName = "Create StageCompetitionConfigSO", fileName = "StageCompetitionConfigSO", order = 0)]
    public class StageCompetitionConfigSO : ScriptableObject
    {
        [field: SerializeField] public StageData[] WavesData { get; private set; }
    }
}