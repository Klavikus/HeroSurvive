using System;
using System.Collections.Generic;
using CodeBase.Domain;
using CodeBase.Domain.Enums;
using CodeBase.MVVM.Models;

namespace CodeBase.MVVM.ViewModels
{
    public class MainPropertiesViewModel
    {
        private readonly PropertiesModel _propertiesModel;
        
        public event Action<IReadOnlyDictionary<BaseProperty, float>> PropertiesChanged;
        
        public IReadOnlyDictionary<BaseProperty, float> BaseProperties { get; private set; }

        public MainPropertiesViewModel(PropertiesModel propertiesModel)
        {
            _propertiesModel = propertiesModel;
            _propertiesModel.Changed += OnPropertiesChanged;
        }

        ~MainPropertiesViewModel() => _propertiesModel.Changed -= OnPropertiesChanged;

        private void OnPropertiesChanged(MainProperties mainProperties)
        {
            BaseProperties = mainProperties.BaseProperties;
            PropertiesChanged?.Invoke(BaseProperties);
        }
    }
}