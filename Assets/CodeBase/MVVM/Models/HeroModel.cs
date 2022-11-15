using System;
using System.Collections.Generic;
using CodeBase.Domain;
using CodeBase.Domain.Data;

namespace CodeBase.MVVM.Models
{
    public class HeroModel
    {
        private List<AdditionalHeroProperty> _additionalProperties = new List<AdditionalHeroProperty>();

        public HeroData CurrentSelectedHero;
        public event Action<HeroData> Changed;

        public void SetHeroData(HeroData heroData)
        {
            CurrentSelectedHero = heroData;
            _additionalProperties = heroData.AdditionalProperties;
            Changed?.Invoke(heroData);
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