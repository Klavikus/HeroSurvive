using System;
using System.Linq;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Api.ViewModels;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;

namespace GameCore.Source.Controllers.Core.ViewModels
{
    public class HeroSelectorViewModel : IHeroSelectorViewModel
    {
        private readonly HeroModel _heroModel;
        private readonly IConfigurationProvider _configurationProvider;
        private readonly HeroData[] _availableHeroesData;
        private readonly IAudioPlayerService _sfxService;
        
        public event Action<HeroData> HeroSelected;
        
        public HeroData CurrentSelectedHeroData { get; private set; }
        public int MaxHeroId => _availableHeroesData.Length - 1;

        public int CurrentSelectedHeroId =>
            _availableHeroesData.TakeWhile(data => data != CurrentSelectedHeroData).Count();

        public HeroSelectorViewModel
        (
            HeroModel heroModel,
            IConfigurationProvider configurationProvider,
            IAudioPlayerService sfxService
        )
        {
            _heroModel = heroModel;
            _configurationProvider = configurationProvider;
            _sfxService = sfxService;
            _availableHeroesData = _configurationProvider.HeroConfig.HeroesData.Select(heroData => heroData).ToArray();
            CurrentSelectedHeroData = _availableHeroesData.First();
        }

        public void SelectHero(HeroData data)
        {
            _heroModel.SetHeroData(data);
            CurrentSelectedHeroData = data;
            HeroSelected?.Invoke(data);
        }

        public void SelectHero(int heroId) =>
            SelectHero(_availableHeroesData[heroId]);


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