using System;
using CodeBase.MVVM.Models;

public class HeroDescriptionViewModel
{
    private readonly HeroModel _heroModel;
    public event Action<HeroData> Changed;

    public HeroDescriptionViewModel(HeroModel heroModel)
    {
        _heroModel = heroModel;
        _heroModel.Changed += OnHeroModelChanged;
    }

    ~HeroDescriptionViewModel() => _heroModel.Changed -= OnHeroModelChanged;

    private void OnHeroModelChanged(HeroData heroData) => Changed?.Invoke(heroData);
}