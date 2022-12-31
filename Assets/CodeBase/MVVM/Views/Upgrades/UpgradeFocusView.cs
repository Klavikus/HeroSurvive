using System.Linq;
using CodeBase.Domain.Data;
using CodeBase.Infrastructure.Factories;
using CodeBase.Infrastructure.Services;
using CodeBase.MVVM.Builders;
using CodeBase.MVVM.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views.Upgrades
{
    public class UpgradeFocusView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private TMP_Text _description;
        [SerializeField] private TMP_Text _upgradePrice;
        [SerializeField] private TMP_Text _sellPrice;
        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _sellButton;
        [SerializeField] private Image _icon;
        [SerializeField] private RectTransform _upgradeLevelViewsContainer;

        private UpgradeLevelView[] _levelUpgradeViews;
        private UpgradeViewModel _upgradeViewModel;
        private UpgradeData _upgradeData;
        private ViewFactory _viewFactory;
        private UpgradeDescriptionBuilder _descriptionBuilder;
        private CurrencyViewModel _currencyViewModel;
        private ITranslationService _translationService;
        private int _currentLevel;

        private void OnEnable()
        {
            if (_upgradeViewModel == null)
                return;
            _upgradeViewModel.Upgraded += OnUpgraded;
            _upgradeViewModel.UpgradeSelected += OnUpgradeSelected;
            _upgradeButton.onClick.AddListener(OnBuyButtonClicked);
            _sellButton.onClick.AddListener(OnResetButtonClicked);
        }

        private void OnDisable()
        {
            if (_upgradeViewModel == null)
                return;
            _upgradeViewModel.Upgraded -= OnUpgraded;
            _upgradeViewModel.UpgradeSelected -= OnUpgradeSelected;
            _upgradeButton.onClick.RemoveListener(OnBuyButtonClicked);
            _sellButton.onClick.RemoveListener(OnResetButtonClicked);
        }

        public void Initialize(
            UpgradeViewModel upgradeViewModel,
            ViewFactory viewFactory,
            CurrencyViewModel currencyViewModel,
            UpgradeDescriptionBuilder descriptionBuilder)
        {
            _translationService = AllServices.Container.AsSingle<ITranslationService>();
            _upgradeViewModel = upgradeViewModel;
            _viewFactory = viewFactory;
            _descriptionBuilder = descriptionBuilder;
            _currencyViewModel = currencyViewModel;

            _upgradeViewModel.Upgraded += OnUpgraded;
            _upgradeViewModel.UpgradeSelected += OnUpgradeSelected;
            _upgradeButton.onClick.AddListener(OnBuyButtonClicked);
            _sellButton.onClick.AddListener(OnResetButtonClicked);
        }

        private void OnUpgraded(UpgradeData upgradeData, int currentLevel)
        {
            for (int i = 0; i < currentLevel; i++)
                _levelUpgradeViews[i].SetSelectedStatus(true);

            UpdateRender(upgradeData, currentLevel);
        }

        private void OnUpgradeSelected(UpgradeData upgradeData, int currentLevel)
        {
            _upgradeData = upgradeData;
            _name.text = _translationService.GetLocalizedText(upgradeData.TranslatableNames);
            _icon.sprite = upgradeData.Icon;
            _currentLevel = currentLevel;
            UpdateRender(_upgradeData, _currentLevel);
        }

        private void UpdateRender(UpgradeData upgradeData, int currentLevel)
        {
            _description.text = _descriptionBuilder.BuildDescriptionText(upgradeData, currentLevel);

            bool isMaxLevel = currentLevel == upgradeData.Upgrades.Length;
            int levelToCheck = currentLevel;

            if (isMaxLevel)
                levelToCheck--;

            bool cantPayUpgrade = !_upgradeViewModel.CheckPay(_upgradeData, levelToCheck);
            bool canReset = currentLevel > 0;

            _upgradeButton.interactable = cantPayUpgrade == false && isMaxLevel == false;
            _sellButton.interactable = canReset;
            PrepareLevelUpgradeViews();
            ShowCurrentLevel(currentLevel);
            UpdatePrices(
                upgradeData.Upgrades,
                currentLevel,
                cantPayUpgrade,
                canReset,
                isMaxLevel);
        }

        private void UpdatePrices(
            UpgradesLevelData[] upgradesData,
            int currentLevel,
            bool cantPayUpgrade,
            bool canReset,
            bool isMaxLevel
        )
        {
            _upgradePrice.text =
                _descriptionBuilder.BuildBuyPriceText(upgradesData, currentLevel, cantPayUpgrade, isMaxLevel);
            _sellPrice.text = _descriptionBuilder.BuildSellPriceText(upgradesData, currentLevel, canReset);
        }

        private void OnBuyButtonClicked() => _upgradeViewModel.Upgrade(_upgradeData);
        private void OnResetButtonClicked() => _upgradeViewModel.Reset(_upgradeData);

        private void ShowCurrentLevel(int currentLevel)
        {
            for (int i = 0; i < currentLevel; i++)
                _levelUpgradeViews[i].SetSelectedStatus(true);
        }

        private void PrepareLevelUpgradeViews()
        {
            _levelUpgradeViews = _upgradeLevelViewsContainer.GetComponentsInChildren<UpgradeLevelView>(true);

            int needed = _upgradeData.Upgrades.Length - _levelUpgradeViews.Length;

            foreach (UpgradeLevelView levelUpgradeView in _levelUpgradeViews)
                levelUpgradeView.gameObject.SetActive(true);

            if (needed > 0)
            {
                UpgradeLevelView[] additionalViews = _viewFactory.CreateUpgradeLevelViews(needed);
                foreach (UpgradeLevelView additionalView in additionalViews)
                    additionalView.transform.SetParent(_upgradeLevelViewsContainer);
                _levelUpgradeViews = _levelUpgradeViews.Concat(additionalViews).ToArray();
            }

            if (needed < 0)
            {
                for (int i = needed; i < 0; i++)
                    _levelUpgradeViews[_levelUpgradeViews.Length + i].gameObject.SetActive(false);
            }

            foreach (UpgradeLevelView levelUpgradeView in _levelUpgradeViews)
                levelUpgradeView.SetSelectedStatus(false);
        }
    }
}