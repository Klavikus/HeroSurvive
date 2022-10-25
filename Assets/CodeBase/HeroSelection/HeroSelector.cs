using UnityEngine;

namespace CodeBase.HeroSelection
{
    public class HeroSelector
    {
        private HeroSelectionSO _selectionConfig;
    }

    [CreateAssetMenu(menuName = "Create HeroSelectionSO", fileName = "HeroSelectionSO", order = 0)]
    public class HeroSelectionSO : ScriptableObject
    {
        [SerializeField] private Hero[] _availableHeroes;
    }
}