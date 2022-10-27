using System;
using System.Collections.Generic;
using CodeBase.HeroSelection;
using CodeBase.MVVM.Views;
using UnityEngine;

namespace CodeBase.MVVM.Models
{
    public class HeroModel
    {
        private string _name;
        private string _description;
        private Sprite _sprite;
        private List<AdditionalHeroProperty> _additionalProperties = new List<AdditionalHeroProperty>();

        private AbilityViewData _abilityViewData;
        
        public event Action<HeroData> Changed;

        public void SetHeroData(HeroData data)
        {
            _name = data.Name;
            _description = data.Description;
            _sprite = data.Sprite;
            _additionalProperties = data.AdditionalProperties;
            _abilityViewData = data.AbilityViewData;
            Changed?.Invoke(data);
        }

        public MainProperties GetMainPropertiesData()
        {
            MainProperties result = new MainProperties();

            foreach (AdditionalHeroProperty heroProperty in _additionalProperties)
                result.UpdateProperty(heroProperty.BaseProperty, heroProperty.Value);

            return result;
        }
    }
}