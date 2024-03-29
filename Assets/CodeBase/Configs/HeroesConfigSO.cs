using CodeBase.Domain;
using CodeBase.Domain.Data;
using CodeBase.Presentation.Views.HeroSelector;
using UnityEngine;

namespace CodeBase.Configs
{
    [CreateAssetMenu(menuName = "Create HeroConfigSO", fileName = "HeroConfigSO", order = 0)]
    public class HeroesConfigSO : ScriptableObject
    {
        [field: SerializeField] public HeroData[] HeroesData { get; private set; }
        [field: SerializeField] public HeroView BaseHeroView { get; private set; }
    }
}