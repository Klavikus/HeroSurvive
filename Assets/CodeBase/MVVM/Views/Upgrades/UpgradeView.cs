using CodeBase.Domain.Data;
using CodeBase.MVVM.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views.Upgrades
{
    public class UpgradeView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text _name;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _selectBorder;
        [SerializeField] private RectTransform _upgradeLevelViewsContainer;

        private UpgradeLevelView[] _levelUpgradeViews;
        private UpgradeViewModel _upgradeViewModel;

        private UpgradeData _upgradeData;

        private void OnDisable()
        {
            if (_upgradeViewModel == null)
                return;

            _upgradeViewModel.Upgraded -= OnUpgraded;
            _upgradeViewModel.UpgradeSelected -= OnUpgradedSelected;
        }

        public void Initialize(
            UpgradeViewModel upgradeViewModel,
            UpgradeData upgradeData,
            UpgradeLevelView[] levelUpgradeViews)
        {
            _upgradeViewModel = upgradeViewModel;
            _upgradeData = upgradeData;
            _levelUpgradeViews = levelUpgradeViews;
           
            _upgradeViewModel.Upgraded += OnUpgraded;
            _upgradeViewModel.UpgradeSelected += OnUpgradedSelected;


            foreach (UpgradeLevelView levelView in _levelUpgradeViews)
            {
                levelView.transform.SetParent(_upgradeLevelViewsContainer);
                levelView.transform.localScale = Vector3.one;
            }

            UpdateLevelsView(_upgradeViewModel.GetCurrentUpgradeLevel(_upgradeData));

            _name.text = upgradeData.Name;
            _icon.sprite = upgradeData.Icon;
        }

        public void OnPointerClick(PointerEventData eventData) => _upgradeViewModel.SelectUpgrade(_upgradeData);

        private void OnUpgradedSelected(UpgradeData upgradeData, int currentLevel) =>
            _selectBorder.enabled = upgradeData == _upgradeData;

        private void OnUpgraded(UpgradeData data, int currentLevel)
        {
            if (data != _upgradeData)
                return;

            UpdateLevelsView(currentLevel);
        }

        private void UpdateLevelsView(int currentLevel)
        {
            for (int i = 0; i < _levelUpgradeViews.Length; i++)
                _levelUpgradeViews[i].SetSelectedStatus(i <= currentLevel - 1);
        }
    }
}