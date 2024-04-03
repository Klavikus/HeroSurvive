using System;
using System.Collections.Generic;
using GameCore.Source.Domain.Configs;
using UnityEngine;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public class HeroData
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public TranslatableString[] TranslatableName { get; private set; }
        [field: SerializeField] public TranslatableString[] TranslatableDescriptions { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        [field: SerializeField] public List<AdditionalHeroProperty> AdditionalProperties { get; private set; }
        [field: SerializeField] public AbilityConfigSO InitialAbilityConfig { get; private set; }
        [field: SerializeField] public GameObject Prefab { get; private set; }
    }
}