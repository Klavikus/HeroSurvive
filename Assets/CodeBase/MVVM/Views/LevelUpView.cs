using System.Collections.Generic;
using CodeBase.Domain.Data;
using CodeBase.MVVM.Builders;
using CodeBase.MVVM.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views
{
    public class LevelUpView : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private AbilityUpgradeView[] _abilityUpgradeViews;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _reRollButton;

        [SerializeField] private TMP_Text _currentLevel;
        [SerializeField] private Image _currentProgress;

        private LevelUpViewModel _levelUpViewModel;
        private Dictionary<AbilityUpgradeView, AbilityUpgradeData> _abilityUpgradeDataByView;
        private AbilityUpgradeData _currentSelectedUpgrade;

        private void OnDisable()
        {
            _levelUpViewModel.LevelChanged -= OnLevelChanged;
            _levelUpViewModel.InvokedViewHide -= Hide;
            _levelUpViewModel.AvailableUpgradesChanged -= ShowAvailableUpgrades;
            _levelUpViewModel.LevelProgressChanged -= ShowProgress;
            _levelUpViewModel.Rerolled -= Show;

            _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
            _reRollButton.onClick.RemoveListener(OnReRollButtonClicked);
            _levelUpViewModel.ResetLevels();
        }

        private void OnReRollButtonClicked()
        {
            _levelUpViewModel.Reroll();
        }

        public void Initialize(LevelUpViewModel levelUpViewModel, UpgradeDescriptionBuilder upgradeDescriptionBuilder)
        {
            Hide();
            _levelUpViewModel = levelUpViewModel;
            _abilityUpgradeDataByView = new Dictionary<AbilityUpgradeView, AbilityUpgradeData>();

            foreach (AbilityUpgradeView upgradeView in _abilityUpgradeViews)
            {
                upgradeView.Initialize(upgradeDescriptionBuilder);
                _abilityUpgradeDataByView.Add(upgradeView, null);
                upgradeView.Selected += OnUpgradeSelected;
            }

            _levelUpViewModel.LevelChanged += OnLevelChanged;
            _levelUpViewModel.InvokedViewHide += Hide;
            _levelUpViewModel.AvailableUpgradesChanged += ShowAvailableUpgrades;
            _levelUpViewModel.LevelProgressChanged += ShowProgress;
            _levelUpViewModel.Rerolled += Show;

            _continueButton.onClick.AddListener(OnContinueButtonClicked);
            _reRollButton.onClick.AddListener(OnReRollButtonClicked);

            _currentLevel.text = $"Level 1";
            _currentProgress.fillAmount = 0;
        }

        private void OnLevelChanged(int newLevel)
        {
            _currentLevel.text = $"Level {newLevel}";

            if (_levelUpViewModel.GetAvailableUpgrades().Length == 0)
                return;

            Show();
        }

        private void ShowProgress(float progressPercent)
        {
            _currentProgress.fillAmount = progressPercent;
        }

        private void ShowAvailableUpgrades(AbilityUpgradeData[] abilityUpgradesData)
        {
            for (int i = 0; i < _abilityUpgradeViews.Length; i++)
            {
                _abilityUpgradeViews[i].Show(abilityUpgradesData[i]);
                _abilityUpgradeDataByView[_abilityUpgradeViews[i]] = abilityUpgradesData[i];
            }
        }

        private void OnUpgradeSelected(AbilityUpgradeView selectedUpgrade)
        {
            _currentSelectedUpgrade = _abilityUpgradeDataByView[selectedUpgrade];
            foreach (AbilityUpgradeView abilityUpgradeView in _abilityUpgradeViews)
                abilityUpgradeView.SetSelected(abilityUpgradeView == selectedUpgrade);
        }

        private void OnContinueButtonClicked()
        {
            if (_currentSelectedUpgrade == null)
            {
                _currentSelectedUpgrade = _abilityUpgradeDataByView[_abilityUpgradeViews[0]];
            }

            _levelUpViewModel.SelectUpgrade(_currentSelectedUpgrade);
            _currentSelectedUpgrade = null;
        }

        private void Show()
        {
            Time.timeScale = 0f;
            _mainCanvas.enabled = true;

            AbilityUpgradeData[] upgradesData = _levelUpViewModel.GetAvailableUpgrades();

            for (int i = 0; i < _abilityUpgradeViews.Length; i++)
            {
                if (i < upgradesData.Length)
                {
                    _abilityUpgradeDataByView[_abilityUpgradeViews[i]] = upgradesData[i];
                    _abilityUpgradeViews[i].Show(upgradesData[i]);
                }
                else
                {
                    _abilityUpgradeDataByView[_abilityUpgradeViews[i]] = null;
                    _abilityUpgradeViews[i].Hide();
                }
            }

            _abilityUpgradeViews[0].SetSelected(true);
        }

        private void Hide()
        {
            Time.timeScale = 1f;
            _mainCanvas.enabled = false;
        }
    }
}