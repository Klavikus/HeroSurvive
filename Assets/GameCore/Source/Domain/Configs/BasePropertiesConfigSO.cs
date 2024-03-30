using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Enums;
using GameCore.Source.Domain.Models;
using UnityEngine;

namespace GameCore.Source.Domain.Configs
{
    [CreateAssetMenu(menuName = "Create BasePropertiesConfigSO", fileName = "BasePropertiesConfigSO", order = 0)]
    public class BasePropertiesConfigSO : ScriptableObject
    {
        [SerializeField] private List<MainPropertyViewData> _data;

        private Dictionary<BaseProperty, MainPropertyViewData> _propertyViewsData;

        [field: SerializeField] public GameObject PropertyView { get; private set; }
        public IReadOnlyList<MainPropertyViewData> PropertiesData => _data;

        private void OnValidate()
        {
            if (_data.Select(data => data.BaseProperty).Distinct().Count() <
                _data.Select(data => data.BaseProperty).Count())
                throw new ArgumentException(
                    $"{typeof(List<MainPropertyViewData>)} should contain distinct property types.");

            _propertyViewsData = new Dictionary<BaseProperty, MainPropertyViewData>();

            foreach (MainPropertyViewData propertyViewData in _data)
                _propertyViewsData.Add(propertyViewData.BaseProperty, propertyViewData);
        }

        public MainProperties GetPropertiesData()
        {
            MainProperties result = new MainProperties();
            foreach (MainPropertyViewData propertyData in _data)
                result.UpdateProperty(propertyData.BaseProperty, propertyData.Value);

            return result;
        }

        public IReadOnlyDictionary<BaseProperty, MainPropertyViewData> GetPropertyViewsData()
        {
            _propertyViewsData = new Dictionary<BaseProperty, MainPropertyViewData>();

            foreach (MainPropertyViewData propertyViewData in _data)
                _propertyViewsData.Add(propertyViewData.BaseProperty, propertyViewData);

            return _propertyViewsData;
        }
    }
}