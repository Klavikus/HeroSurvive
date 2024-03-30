using GameCore.Source.Domain.Data;
using UnityEngine;

namespace GameCore.Source.Domain.Configs
{
    [CreateAssetMenu(menuName = "Create LevelViewSo", fileName = "LevelViewSo", order = 0)]
    public class LevelViewSo : ScriptableObject
    {
        [field: SerializeField] public LevelViewData[] LevelViewsData;
    }
}