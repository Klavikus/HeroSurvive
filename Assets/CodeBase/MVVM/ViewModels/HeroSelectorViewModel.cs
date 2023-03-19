using System;
using System.Linq;
using CodeBase.Domain.Data;
using CodeBase.Extensions;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Models;
using UnityEngine;

namespace CodeBase.MVVM.ViewModels
{
    public class HeroSelectorViewModel
    {
        private readonly HeroModel _heroModel;
        private readonly MenuModel _menuModel;
        private readonly GameLoopModel _gameLoopModel;
        private readonly ITranslationService _translationService;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly HeroData[] _availableHeroesData;
        public event Action<HeroData> HeroSelected;
        public event Action HeroSelectorEnabled;
        public event Action HeroSelectorDisabled;
        public HeroData CurrentSelectedHeroData { get; private set; }
        public int MaxHeroId => _availableHeroesData.Length - 1;

        public int CurrentSelectedHeroId =>
            _availableHeroesData.TakeWhile(data => data != CurrentSelectedHeroData).Count();

        public HeroSelectorViewModel
        (
            HeroModel heroModel,
            MenuModel menuModel,
            GameLoopModel gameLoopModel,
            ITranslationService translationService,
            IConfigurationProvider configurationProvider
        )
        {
            _heroModel = heroModel;
            _menuModel = menuModel;
            _gameLoopModel = gameLoopModel;
            _translationService = translationService;
            _configurationProvider = configurationProvider;
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

        public void Continue() => _gameLoopModel.InvokeStartLevel(CurrentSelectedHeroData);

        private void OnMenuModelHeroSelectorEnabled() => HeroSelectorEnabled?.Invoke();

        private void OnMenuModelHeroSelectorDisabled() => HeroSelectorDisabled?.Invoke();


        public void HandleMove(int dX, int dY, int rowCount, int colCount)
        {
            if (dX != 0)
                HandleHorizontalScroll(dX, rowCount, colCount);

            if (dY != 0)
                HandleVerticalScroll(dY, rowCount, colCount);
        }

        private void HandleVerticalScroll(int dY, int rowCount, int colCount)
        {
            int[] currentPosition = ConvertIndexFromLinear(CurrentSelectedHeroId, rowCount, colCount);
            int newY = currentPosition[0] + dY;

            int newLinearIndex = ConvertIndexToLinear(currentPosition[1], newY, colCount);

            if (newLinearIndex.ContainsInInterval(0, MaxHeroId))
                SelectHero(newLinearIndex);
        }

        private void HandleHorizontalScroll(int dX, int rowCount, int colCount)
        {
            int newLinearIndex = CurrentSelectedHeroId + dX;

            if (newLinearIndex.ContainsInInterval(0, MaxHeroId))
                SelectHero(newLinearIndex);
        }

        private int[] ConvertIndexFromLinear(int index, int numRows, int numCols)
        {
            if (index == 0)
                return new[] {0, 0};
            int row = Mathf.FloorToInt((1f + index) / numRows) - 1;
            int col = index % numCols;
            return new[] {row, col};
        }

        private int ConvertIndexToLinear(int x, int y, int numCols) => y * numCols + x;
    }
}