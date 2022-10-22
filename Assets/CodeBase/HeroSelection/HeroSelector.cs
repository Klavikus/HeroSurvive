using System;
using CodeBase.Abilities;
using UnityEngine;

namespace CodeBase.HeroSelection
{
    public class HeroSelector
    {
        private HeroSelectionSO _selectionConfig;
    }

    [Serializable]
    public class Hero
    {
        [SerializeField] private string _name;
        [SerializeField] private int _price;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private AbilityConfigSO _initialAbilityConfig;
    }

    [CreateAssetMenu(menuName = "Create HeroSelectionSO", fileName = "HeroSelectionSO", order = 0)]
    public class HeroSelectionSO : ScriptableObject
    {
        [SerializeField] private Hero[] _availableHeroes;
        
    }
}