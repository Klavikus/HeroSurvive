using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.MVVM.Views;
using UnityEngine;

namespace CodeBase.HeroSelection
{
    [CreateAssetMenu(menuName = "Create BasePropertiesConfigSO", fileName = "BasePropertiesConfigSO", order = 0)]
    public class BasePropertiesConfigSO : ScriptableObject
    {
        [SerializeField] private List<MainPropertyViewData> _data;

        [field: SerializeField] public PropertyView PropertyView { get; private set; }
        public IReadOnlyList<MainPropertyViewData> PropertiesData => _data;

        private void OnValidate()
        {
            if (_data.Select(data => data.BaseProperty).Distinct().Count() <
                _data.Select(data => data.BaseProperty).Count())
                throw new ArgumentException(
                    $"{typeof(List<MainPropertyViewData>)} should contain distinct property types.");
        }

        public MainProperties GetPropertiesData()
        {
            MainProperties result = new MainProperties();
            foreach (MainPropertyViewData propertyData in _data)
                result.UpdateProperty(propertyData.BaseProperty, propertyData.Value);
            return result;
        }
    }
}