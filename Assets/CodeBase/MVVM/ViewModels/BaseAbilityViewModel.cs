using System;
using CodeBase.MVVM.Models;

namespace CodeBase.MVVM.ViewModels
{
    public class BaseAbilityViewModel
    {
        private readonly HeroModel _heroModel;

        public event Action<HeroData> Changed;

        public BaseAbilityViewModel(HeroModel heroModel)
        {
            _heroModel = heroModel;
            _heroModel.Changed += OnHeroChanged;
        }

        ~BaseAbilityViewModel() => _heroModel.Changed -= OnHeroChanged;

        private void OnHeroChanged(HeroData heroData) => Changed?.Invoke(heroData);
    }
}