using System;
using CodeBase.Domain;

namespace CodeBase.MVVM.Models
{
    public class PropertiesModel
    {
        private MainProperties _mainProperties;
        
        public event Action<MainProperties> Changed;
        
        public void SetResultProperties(MainProperties mainProperties)
        {
            _mainProperties = mainProperties;
            Changed?.Invoke(_mainProperties);
        }
    }
}