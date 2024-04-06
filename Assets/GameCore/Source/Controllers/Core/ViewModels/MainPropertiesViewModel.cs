using System;
using System.Collections.Generic;
using GameCore.Source.Domain.Enums;
using GameCore.Source.Domain.Models;

namespace GameCore.Source.Controllers.Core.ViewModels
{
    public class MainPropertiesViewModel
    {
        private readonly PropertiesModel _propertiesModel;

        public event Action<IReadOnlyDictionary<BaseProperty, float>> PropertiesChanged;

        public IReadOnlyDictionary<BaseProperty, float> BaseProperties => _propertiesModel.MainProperties;

        public MainPropertiesViewModel(PropertiesModel propertiesModel)
        {
            _propertiesModel = propertiesModel;
            _propertiesModel.Changed += OnPropertiesChanged;
        }

        ~MainPropertiesViewModel()
        {
            _propertiesModel.Changed -= OnPropertiesChanged;
        }

        private void OnPropertiesChanged(MainProperties mainProperties) =>
            PropertiesChanged?.Invoke(BaseProperties);
    }
}