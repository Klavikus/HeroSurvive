using System;
using System.Collections.Generic;
using CodeBase.Configs;
using CodeBase.Domain.EntityComponents;
using CodeBase.Domain.Enums;
using UnityEngine;

namespace CodeBase.Domain.Data
{
    [Serializable]
    public class TranslatableString
    {
        [field: SerializeField] public Language Language { get; private set; }
        [field: SerializeField] public string Text { get; private set; }

        public TranslatableString(Language language, string text)
        {
            Language = language;
            Text = text;
        }

        public override string ToString() => $"{Language} {Text}";
    }

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
        [field: SerializeField] public Player Prefab { get; private set; }
    }

    [Serializable]
    public struct AdditionalHeroProperty
    {
        public BaseProperty BaseProperty;
        public float Value;

        public AdditionalHeroProperty(BaseProperty baseProperty, float value)
        {
            BaseProperty = baseProperty;
            Value = value;
        }
    }
}