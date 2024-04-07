using System;
using System.Linq;
using GameCore.Source.Controllers.Api.Factories;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Data;
using GameCore.Source.Presentation.Api;
using GameCore.Source.Presentation.Api.Factories;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Presenters.MainMenu
{
    public class UpgradeFocusPresenter : IPresenter
    {
        private readonly IUpgradeFocusView _view;
        private readonly IUpgradeLevelView[] _upgradeLevelViews;
        private readonly IPersistentUpgradeService _persistentUpgradeService;
        private readonly IUpgradeDescriptionBuilder _descriptionBuilder;
        private readonly IPersistentUpgradeLevelViewFactory _persistentUpgradeLevelViewFactory;
        private readonly ILocalizationService _localizationService;

        private UpgradeData _currentUpgradeData;
        private IUpgradeLevelView[] _levelUpgradeViews;
        private int _currentLevel;

        public UpgradeFocusPresenter(
            IUpgradeFocusView view,
            IPersistentUpgradeService persistentUpgradeService,
            IUpgradeDescriptionBuilder descriptionBuilder,
            IPersistentUpgradeLevelViewFactory persistentUpgradeLevelViewFactory,
            ILocalizationService localizationService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _persistentUpgradeService = persistentUpgradeService ??
                                        throw new ArgumentNullException(nameof(persistentUpgradeService));
            _descriptionBuilder = descriptionBuilder ?? throw new ArgumentNullException(nameof(descriptionBuilder));
            _persistentUpgradeLevelViewFactory = persistentUpgradeLevelViewFactory ??
                                                 throw new ArgumentNullException(
                                                     nameof(persistentUpgradeLevelViewFactory));
            _localizationService = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
        }

        public void Enable()
        {
            _view.Initialize();
            _view.UpgradeButton.Clicked += OnUpgradeButtonClicked;
            _view.SellButton.Clicked += OnSellButtonClicked;

            _currentUpgradeData = _persistentUpgradeService.GetCurrentUpgradeData();
            OnUpgradeSelected(_currentUpgradeData,
                _persistentUpgradeService.GetCurrentUpgradeLevel(_currentUpgradeData));
            _persistentUpgradeService.Upgraded += OnUpgraded;
            _persistentUpgradeService.UpgradeSelected += OnUpgradeSelected;
        }

        public void Disable()
        {
            _view.UpgradeButton.Clicked -= OnUpgradeButtonClicked;
            _view.SellButton.Clicked -= OnSellButtonClicked;

            _persistentUpgradeService.Upgraded -= OnUpgraded;
            _persistentUpgradeService.UpgradeSelected -= OnUpgradeSelected;
        }

        private void OnUpgradeButtonClicked() =>
            _persistentUpgradeService.Upgrade(_currentUpgradeData);

        private void OnSellButtonClicked() =>
            _persistentUpgradeService.Reset(_currentUpgradeData);

        private void OnUpgradeSelected(UpgradeData upgradeData, int currentLevel)
        {
            _currentUpgradeData = upgradeData;
            _view.Name.text = _localizationService.GetLocalizedText(upgradeData.TranslatableNames);
            _view.Icon.sprite = upgradeData.Icon;
            _currentLevel = currentLevel;
            UpdateRender(_currentUpgradeData, _currentLevel);
        }

        private void OnUpgraded(UpgradeData upgradeData, int currentLevel)
        {
            UpdateRender(upgradeData, currentLevel);
        }

        private void UpdateRender(UpgradeData upgradeData, int currentLevel)
        {
            _view.Description.text = _descriptionBuilder.BuildDescriptionText(upgradeData, currentLevel);

            bool isMaxLevel = currentLevel == upgradeData.Upgrades.Length;
            int levelToCheck = currentLevel;

            if (isMaxLevel)
                levelToCheck--;

            bool cantPayUpgrade = !_persistentUpgradeService.CheckPay(_currentUpgradeData, levelToCheck);
            bool canReset = currentLevel > 0;

            _view.UpgradeButton.SetInteractionLock(cantPayUpgrade || isMaxLevel);
            _view.SellButton.SetInteractionLock(canReset == false);

            PrepareLevelUpgradeViews();

            UpdatePrices(
                upgradeData.Upgrades,
                currentLevel,
                cantPayUpgrade,
                canReset,
                isMaxLevel);
        }

        private void PrepareLevelUpgradeViews()
        {
            _levelUpgradeViews = _view.UpgradeLevelViewsContainer.GetComponentsInChildren<IUpgradeLevelView>(true);

            int needed = _currentUpgradeData.Upgrades.Length - _levelUpgradeViews.Length;
            int maxLevel = _currentUpgradeData.Upgrades.Length;

            foreach (IUpgradeLevelView levelUpgradeView in _levelUpgradeViews)
                levelUpgradeView.Show();

            if (needed > 0)
            {
                IUpgradeLevelView[] additionalViews = _persistentUpgradeLevelViewFactory.Create(needed);

                foreach (IUpgradeLevelView additionalView in additionalViews)
                {
                    additionalView.Transform.SetParent(_view.UpgradeLevelViewsContainer);
                    additionalView.Transform.localScale = Vector3.one;
                }

                _levelUpgradeViews = _levelUpgradeViews.Concat(additionalViews).ToArray();
            }

            for (var i = 0; i < _levelUpgradeViews.Length; i++)
            {
                if (i <= maxLevel)
                    _levelUpgradeViews[i].Show();
                else
                    _levelUpgradeViews[i].Hide();

                _levelUpgradeViews[i]
                    .SetSelectedStatus(_persistentUpgradeService.GetCurrentUpgradeLevel(_currentUpgradeData) > i);
            }
        }

        private void UpdatePrices(
            UpgradesLevelData[] upgradesData,
            int currentLevel,
            bool cantPayUpgrade,
            bool canReset,
            bool isMaxLevel
        )
        {
            _view.UpgradePrice.text =
                _descriptionBuilder.BuildBuyPriceText(upgradesData, currentLevel, cantPayUpgrade, isMaxLevel);
            _view.SellPrice.text = _descriptionBuilder.BuildSellPriceText(upgradesData, currentLevel, canReset);
        }
    }
}