using UnityEngine;

namespace GameCore.Source.Domain.Data
{
    [CreateAssetMenu(menuName = "Create SlicerStageInfo", fileName = "SlicerStageInfo", order = 0)]
    public class SlicerStageInfo : ScriptableObject
    {
        [field: SerializeField] public SlicePairData[] SicePairs { get; set; }
    }
}