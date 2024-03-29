using System;
using CodeBase.Domain.Enums;

namespace CodeBase.Domain.Data
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