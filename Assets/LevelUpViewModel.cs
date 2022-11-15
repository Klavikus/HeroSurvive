using System;

public class LevelUpViewModel
{
    private readonly LevelUpModel _levelUpModel;
    public event Action<int> LevelChanged;
    public event Action InvokedViewHide;
    public event Action<AbilityUpgradeData[]> AvailableUpgradesChanged;
    public event Action<float> LevelProgressChanged;

    public LevelUpViewModel(LevelUpModel levelUpModel)
    {
        _levelUpModel = levelUpModel;
        _levelUpModel.LevelChanged += OnLevelChanged;
        _levelUpModel.AvailableUpgradesChanged += OnAvailableUpgradesChanged;
        _levelUpModel.LevelProgressChanged += OnLevelProgressChanged;
        _levelUpModel.UpgradeSelected += OnUpgradeSelected;
    }

    private void OnUpgradeSelected(AbilityUpgradeData obj) => InvokedViewHide?.Invoke();

    private void OnAvailableUpgradesChanged(AbilityUpgradeData[] availableUpgrades) =>
        AvailableUpgradesChanged?.Invoke(availableUpgrades);

    private void OnLevelChanged(int currentLevel) => LevelChanged?.Invoke(currentLevel);
    private void OnLevelProgressChanged(float currentPercent) => LevelProgressChanged?.Invoke(currentPercent);

    public void SelectUpgrade(AbilityUpgradeData abilityUpgradeData) => _levelUpModel.SelectUpgrade(abilityUpgradeData);

    public void ResetLevels()
    {
        _levelUpModel.ResetLevels();
    }
}