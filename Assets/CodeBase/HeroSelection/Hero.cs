using System;
using CodeBase.Abilities;
using UnityEngine;

namespace CodeBase.HeroSelection
{
    [Serializable]
    public class Hero
    {
        [SerializeField] private string _name;
        [SerializeField] private int _price;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private AbilityConfigSO _initialAbilityConfig;
    }
}