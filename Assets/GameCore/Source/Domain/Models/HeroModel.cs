using System;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Domain.Models
{
    public class HeroModel
    {
        private readonly MainProperties _mainProperties = new MainProperties();

        public event Action<HeroData> Changed;

        public HeroData CurrentSelectedHero { get; private set; }
        public MainProperties GetMainPropertiesData() => _mainProperties;

        public void SetHeroData(HeroData heroData)
        {
            CurrentSelectedHero = heroData;

            foreach (AdditionalHeroProperty heroProperty in heroData.AdditionalProperties)
                _mainProperties.UpdateProperty(heroProperty.BaseProperty, heroProperty.Value);

            Changed?.Invoke(heroData);
        }
    }
}