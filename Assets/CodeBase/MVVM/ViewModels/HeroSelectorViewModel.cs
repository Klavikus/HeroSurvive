using System;
using CodeBase.MVVM.Models;

namespace CodeBase.MVVM.ViewModels
{
    public class HeroSelectorViewModel
    {
        private readonly HeroModel _heroModel;

        public event Action<HeroData> HeroSelected;

        public HeroSelectorViewModel(HeroModel heroModel)
        {
            _heroModel = heroModel;
        }

        public void SelectHero(HeroData data)
        {
            _heroModel.SetHeroData(data);
            HeroSelected?.Invoke(data);
        }
    }
}