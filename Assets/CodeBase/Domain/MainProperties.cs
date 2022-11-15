using System;
using System.Collections.Generic;
using CodeBase.Domain.Enums;

namespace CodeBase.Domain
{
    public class MainProperties
    {
        private readonly Dictionary<BaseProperty, float> _baseProperties;
        public IReadOnlyDictionary<BaseProperty, float> BaseProperties => _baseProperties;

        public static MainProperties operator +(MainProperties a, MainProperties b)
        {
            if (a == null && b == null)
                return null;

            if (a == null)
                return b;

            if (b == null)
                return a;

            MainProperties result = new MainProperties();
            foreach (BaseProperty propertiesKey in a.BaseProperties.Keys)
                result.UpdateProperty(propertiesKey, a.BaseProperties[propertiesKey] + b.BaseProperties[propertiesKey]);
            return result;
        }

        public static MainProperties operator -(MainProperties a, MainProperties b)
        {
            if (a == null && b == null)
                return null;

            if (b == null)
                return a;

            MainProperties result = new MainProperties();


            if (a == null)
                foreach (BaseProperty propertiesKey in b.BaseProperties.Keys)
                    result.UpdateProperty(propertiesKey, -b.BaseProperties[propertiesKey]);
            else
                foreach (BaseProperty propertiesKey in a.BaseProperties.Keys)
                    result.UpdateProperty(propertiesKey,
                        a.BaseProperties[propertiesKey] - b.BaseProperties[propertiesKey]);
           
            return result;
        }

        public MainProperties()
        {
            _baseProperties = new Dictionary<BaseProperty, float>();
            foreach (BaseProperty baseProperty in Enum.GetValues(typeof(BaseProperty)))
                _baseProperties.Add(baseProperty, 0);
        }

        public void UpdateProperty(BaseProperty baseProperty, float baseValue) => _baseProperties[baseProperty] = baseValue;
    }
}