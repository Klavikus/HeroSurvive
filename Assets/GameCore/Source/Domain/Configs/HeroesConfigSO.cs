using GameCore.Source.Domain.Data;
using UnityEngine;

namespace GameCore.Source.Domain.Configs
{
    [CreateAssetMenu(menuName = "Create HeroConfigSO", fileName = "HeroConfigSO", order = 0)]
    public class HeroesConfigSO : ScriptableObject
    {
        [field: SerializeField] public HeroData[] HeroesData { get; private set; }
        [field: SerializeField] public GameObject BaseHeroView { get; private set; }
    }
}