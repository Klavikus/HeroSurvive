using System;
using System.Linq;
using CodeBase.GameCore.Domain.Data;
using CodeBase.GameCore.Domain.Models;
using CodeBase.GameCore.Infrastructure.Services;
using CodeBase.Utilities.Extensions;

namespace CodeBase.GameCore.Presentation.ViewModels
{
    public class HeroSelectorViewModel
    {
        private readonly HeroModel _heroModel;
        private readonly MenuModel _menuModel;
        private readonly GameLoopModel _gameLoopModel;
        private readonly ITranslationService _translationService;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly HeroData[] _availableHeroesData;
        private IAudioPlayerService _sfxService;
        public event Action<HeroData> HeroSelected;
        public event Action HeroSelectorEnabled;
        public event Action HeroSelectorDisabled;
        public HeroData CurrentSelectedHeroData { get; private set; }
        private int MaxHeroId => _availableHeroesData.Length - 1;

        public int CurrentSelectedHeroId =>
            _availableHeroesData.TakeWhile(data => data != CurrentSelectedHeroData).Count();

        public HeroSelectorViewModel
        (
            HeroModel heroModel,
            MenuModel menuModel,
            GameLoopModel gameLoopModel,
            ITranslationService translationService,
            IConfigurationProvider configurationProvider,
            IAudioPlayerService sfxService
        )
        {
            _heroModel = heroModel;
            _menuModel = menuModel;
            _gameLoopModel = gameLoopModel;
            _translationService = translationService;
            _configurationProvider = configurationProvider;
            _sfxService = sfxService;
            _availableHeroesData = _configurationProvider.HeroConfig.HeroesData.Select(heroData => heroData).ToArray();
            _menuModel.OpenedHeroSelection += OnMenuModelHeroSelectorEnabled;
            _menuModel.ClosedHeroSelection += OnMenuModelHeroSelectorDisabled;
            _translationService.LocalizationChanged += OnLocalizationChanged;
        }

        private void OnLocalizationChanged()
        {
            HeroSelected?.Invoke(CurrentSelectedHeroData);
        }

        public void SelectHero(HeroData data)
        {
            _heroModel.SetHeroData(data);
            CurrentSelectedHeroData = data;
            HeroSelected?.Invoke(data);
        }

        public void SelectHero(int heroId) => SelectHero(_availableHeroesData[heroId]);

        public void DisableHeroSelector() => _menuModel.DisableHeroSelector();

        public void Continue()
        {
            _sfxService.PlayStartLevel();
            _gameLoopModel.InvokeStartLevel(CurrentSelectedHeroData);
        }

        private void OnMenuModelHeroSelectorEnabled() => HeroSelectorEnabled?.Invoke();

        private void OnMenuModelHeroSelectorDisabled() => HeroSelectorDisabled?.Invoke();


        public void HandleMove(int dX, int dY, int rowCount, int colCount)
        {
            if (dX != 0)
                HandleHorizontalScroll(dX);

            if (dY != 0)
                HandleVerticalScroll(dY, rowCount, colCount);
        }

        private void HandleVerticalScroll(int dY, int rowCount, int colCount)
        {
            int[] currentPosition = CurrentSelectedHeroId.ConvertIndexFromLinear(rowCount, colCount);

            int newLinearIndex = new[] {currentPosition[0] + dY, currentPosition[1]}.ConvertIndexToLinear(colCount);

            if (newLinearIndex.ContainsInInterval(0, MaxHeroId))
                SelectHero(newLinearIndex);
        }

        private void HandleHorizontalScroll(int dX)
        {
            int newLinearIndex = CurrentSelectedHeroId + dX;

            if (newLinearIndex.ContainsInInterval(0, MaxHeroId))
                SelectHero(newLinearIndex);
        }
    }
}