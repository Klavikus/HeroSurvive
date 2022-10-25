using System;
using System.Collections.Generic;
using CodeBase.Enums;

namespace CodeBase.HeroSelection
{
    public class MainProperties
    {
        private readonly Dictionary<BaseProperty, float> _baseProperties;
        public IReadOnlyDictionary<BaseProperty, float> BaseProperties => _baseProperties;

        public static MainProperties operator +(MainProperties a, MainProperties b)
        {
            MainProperties result = new MainProperties();
            foreach (BaseProperty propertiesKey in a.BaseProperties.Keys)
                result.UpdateProperty(propertiesKey, a.BaseProperties[propertiesKey] + b.BaseProperties[propertiesKey]);
            return result;
        }

        public static MainProperties operator -(MainProperties a, MainProperties b)
        {
            MainProperties result = new MainProperties();
            foreach (BaseProperty propertiesKey in a.BaseProperties.Keys)
                result.UpdateProperty(propertiesKey, a.BaseProperties[propertiesKey] - b.BaseProperties[propertiesKey]);
            return result;
        }

        public MainProperties()
        {
            _baseProperties = new Dictionary<BaseProperty, float>();
            foreach (BaseProperty baseProperty in Enum.GetValues(typeof(BaseProperty)))
                _baseProperties.Add(baseProperty, 0);
        }

        public void UpdateProperty(BaseProperty baseProperty, float baseValue)
        {
            _baseProperties[baseProperty] = baseValue;
        }
    }
}