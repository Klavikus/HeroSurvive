using System;
using CodeBase.GameCore.Domain.Enums;

namespace CodeBase.GameCore.Domain.Data
{
    [Serializable]
    public struct AdditionalHeroProperty
    {
        //TODO: Refactor this
        public BaseProperty BaseProperty;
        public float Value;

        public AdditionalHeroProperty(BaseProperty baseProperty, float value)
        {
            BaseProperty = baseProperty;
            Value = value;
        }
    }
}