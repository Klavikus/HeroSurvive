using System;
using GameCore.Source.Domain.Enums;
using UnityEngine;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public struct AdditionalHeroProperty
    {
        public AdditionalHeroProperty(BaseProperty baseProperty, float value)
        {
            BaseProperty = baseProperty;
            Value = value;
        }

        [field: SerializeField] public BaseProperty BaseProperty { get; private set; }
        [field: SerializeField] public float Value { get; private set; }
    }
}