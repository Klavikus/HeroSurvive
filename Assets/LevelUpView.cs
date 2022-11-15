using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        _continueButton.onClick.RemoveListener(OnContinueButtonClicked);
        _levelUpViewModel.ResetLevels();
    }

    public void Initialize(LevelUpViewModel levelUpViewModel)
    {
        Hide();
        _levelUpViewModel = levelUpViewModel;
        _abilityUpgradeDataByView = new Dictionary<AbilityUpgradeView, AbilityUpgradeData>();

        foreach (AbilityUpgradeView upgradeView in _abilityUpgradeViews)
        {
            _abilityUpgradeDataByView.Add(upgradeView, null);
            upgradeView.Selected += OnUpgradeSelected;
        }

        _levelUpViewModel.LevelChanged += OnLevelChanged;
        _levelUpViewModel.InvokedViewHide += Hide;
        _levelUpViewModel.AvailableUpgradesChanged += ShowAvailableUpgrades;
        _levelUpViewModel.LevelProgressChanged += ShowProgress;

        _continueButton.onClick.AddListener(OnContinueButtonClicked);

        _currentLevel.text = $"Level 1";
        _currentProgress.fillAmount = 0;
    }

    private void OnLevelChanged(int newLevel)
    {
        Show();
        _currentLevel.text = $"Level {newLevel}";
    }

    private void ShowProgress(float progressPercent)
    {
        _currentProgress.fillAmount = progressPercent;
    }

    private void ShowAvailableUpgrades(AbilityUpgradeData[] abilityUpgradesData)
    {
        for (int i = 0; i < _abilityUpgradeViews.Length; i++)
        {
            _abilityUpgradeViews[i].Show(abilityUpgradesData[i].ViewData);
            _abilityUpgradeDataByView[_abilityUpgradeViews[i]] = abilityUpgradesData[i];
        }
    }

    private void OnUpgradeSelected(AbilityUpgradeView selectedUpgrade) =>
        _currentSelectedUpgrade = _abilityUpgradeDataByView[selectedUpgrade];

    private void OnContinueButtonClicked() => _levelUpViewModel.SelectUpgrade(_currentSelectedUpgrade);

    private void Show()
    {
        Time.timeScale = 0f;
        _mainCanvas.enabled = true;
    }

    private void Hide()
    {
        Time.timeScale = 1f;
        _mainCanvas.enabled = false;
    }
}