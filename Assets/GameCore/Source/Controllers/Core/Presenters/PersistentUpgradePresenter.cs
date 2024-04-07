using System;
using Codice.Client.BaseCommands;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Data;
using GameCore.Source.Infrastructure.Api;
using GameCore.Source.Presentation.Api;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.MVPPassiveView.Runtime;
using UnityEngine;

namespace GameCore.Source.Controllers.Core.Presenters
{
    public class PersistentUpgradePresenter : IPresenter
    {
        private readonly IPersistentUpgradeView _view;
        private readonly IPersistentUpgradeService _persistentUpgradeService;
        private readonly ILocalizationService _localizationService;
        private readonly UpgradeData _upgradeData;
        private IUpgradeLevelView[] _upgradeLevelViews;

        public PersistentUpgradePresenter(
            IPersistentUpgradeView view,
            IPersistentUpgradeService persistentUpgradeService,
            IPersistentUpgradeLevelViewFactory persistentUpgradeLevelViewFactory,
            ILocalizationService localizationService,
            UpgradeData upgradeData)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _persistentUpgradeService = persistentUpgradeService;
            _localizationService = localizationService;
            _upgradeData = upgradeData;

            _upgradeLevelViews = persistentUpgradeLevelViewFactory.Create(_upgradeData.Upgrades.Length);

            foreach (IUpgradeLevelView levelView in _upgradeLevelViews)
            {
                levelView.Transform.SetParent(_view.UpgradeLevelViewsContainer);
                levelView.Transform.localScale = Vector3.one;
            }
        }

        public void Enable()
        {
            _persistentUpgradeService.Upgraded += OnUpgraded;
            _persistentUpgradeService.UpgradeSelected += OnUpgradeSelected;
            _localizationService.Changed += OnLocalizationChanged;

            UpdateLevelsView(_persistentUpgradeService.GetCurrentUpgradeLevel(_upgradeData));

            _view.Name.text = _localizationService.GetLocalizedText(_upgradeData.TranslatableNames);
            _view.Icon.sprite = _upgradeData.Icon;
            _view.Clicked += OnViewClicked;
        }

        public void Disable()
        {
            _persistentUpgradeService.Upgraded -= OnUpgraded;
            _persistentUpgradeService.UpgradeSelected -= OnUpgradeSelected;
            _localizationService.Changed -= OnLocalizationChanged;
            _view.Clicked -= OnViewClicked;
        }

        private void OnUpgraded(UpgradeData upgradeData, int currentLevel)
        {
            if (upgradeData != _upgradeData)
                return;

            UpdateLevelsView(currentLevel);
        }

        private void OnUpgradeSelected(UpgradeData upgradeData, int currentLevel) =>
            _view.SelectionBorder.enabled = upgradeData == _upgradeData;

        private void UpdateLevelsView(int currentLevel)
        {
            for (int i = 0; i < _upgradeLevelViews.Length; i++)
                _upgradeLevelViews[i].SetSelectedStatus(i <= currentLevel - 1);
        }

        private void OnLocalizationChanged() =>
            _view.Name.text = _localizationService.GetLocalizedText(_upgradeData.TranslatableNames);

        private void OnViewClicked() =>
            _persistentUpgradeService.SelectUpgrade(_upgradeData);
    }
}