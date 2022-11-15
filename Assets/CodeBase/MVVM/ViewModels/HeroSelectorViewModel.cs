using System;
using CodeBase.Domain.Data;
using CodeBase.MVVM.Models;

namespace CodeBase.MVVM.ViewModels
{
    public class HeroSelectorViewModel
    {
        private readonly HeroModel _heroModel;
        private readonly MenuModel _menuModel;
        private readonly GameLoopModel _gameLoopModel;
        public event Action<HeroData> HeroSelected;
        public event Action HeroSelectorEnabled;
        public event Action HeroSelectorDisabled;
        public HeroData CurrentSelectedHeroData { get; private set; }

        public HeroSelectorViewModel(HeroModel heroModel, MenuModel menuModel, GameLoopModel gameLoopModel)
        {
            _heroModel = heroModel;
            _menuModel = menuModel;
            _gameLoopModel = gameLoopModel;
            _menuModel.OpenedHeroSelection += OnMenuModelHeroSelectorEnabled;
            _menuModel.ClosedHeroSelection += OnMenuModelHeroSelectorDisabled;
        }

        public void SelectHero(HeroData data)
        {
            _heroModel.SetHeroData(data);
            CurrentSelectedHeroData = data;
            HeroSelected?.Invoke(data);
        }

        public void DisableHeroSelector() => _menuModel.DisableHeroSelector();
        public void Continue() => _gameLoopModel.InvokeStartLevel(CurrentSelectedHeroData);
        private void OnMenuModelHeroSelectorEnabled() => HeroSelectorEnabled?.Invoke();
        private void OnMenuModelHeroSelectorDisabled() => HeroSelectorDisabled?.Invoke();
    }
}