using System;
using System.Collections.Generic;
using CodeBase.Domain.Enums;

namespace CodeBase.Domain.Models
{
    public class PropertiesModel
    {
        private MainProperties _mainProperties;
        public event Action<MainProperties> Changed;
        public IReadOnlyDictionary<BaseProperty, float> MainProperties => _mainProperties.BaseProperties;

        public void SetResultProperties(MainProperties mainProperties)
        {
            _mainProperties = mainProperties;
            Changed?.Invoke(_mainProperties);
        }
    }
}