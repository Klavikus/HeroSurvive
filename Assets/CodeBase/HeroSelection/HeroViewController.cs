using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.HeroSelectionService;
using UnityEngine;

public class HeroViewController : MonoBehaviour
{
    [SerializeField] private Transform _viewContainer;
    [SerializeField] private HeroDescriptionView _descriptionView;

    private HeroesConfigSO _heroesConfigSO;
    private Dictionary<HeroView, HeroData> _heroViews = new Dictionary<HeroView, HeroData>();

    public event Action<HeroData> HeroSelected;
    
    private void OnDisable()
    {
        foreach (HeroView view in _heroViews.Keys)
            view.Clicked -= OnViewClicked;
    }

    public void Initialize(IConfigurationProvider configurationProvider)
    {
        _heroesConfigSO = configurationProvider.GetHeroConfig();
        CreateView();
    }

    private void CreateView()
    {
        foreach (HeroData heroData in _heroesConfigSO.HeroesData)
        {
            HeroView heroView = Instantiate(_heroesConfigSO.BaseHeroView, Vector3.zero, Quaternion.identity,
                _viewContainer);
            heroView.Initialize(heroData.Sprite);
            heroView.Clicked += OnViewClicked;
            _heroViews.Add(heroView, heroData);
        }

        OnViewClicked(_heroViews.First().Key);
    }

    private void OnViewClicked(HeroView view)
    {
        foreach (HeroView heroView in _heroViews.Keys)
        {
            if (heroView == view)
                heroView.Select();
            else
                heroView.Deselect();
        }

        _descriptionView.Render(_heroViews[view]);
        HeroSelected?.Invoke(_heroViews[view]);
    }
}