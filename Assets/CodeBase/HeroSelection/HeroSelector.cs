using UnityEngine;

namespace CodeBase.HeroSelection
{
    [CreateAssetMenu(menuName = "Create HeroSelectionSO", fileName = "HeroSelectionSO", order = 0)]
    public class HeroSelectionSO : ScriptableObject
    {
        [SerializeField] private Hero[] _availableHeroes;
    }
}