using CodeBase.Infrastructure.Factories;
using CodeBase.MVVM.Builders;
using CodeBase.MVVM.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views.Upgrades
{
    public class UpgradesSelectorView : MonoBehaviour
    {
        [SerializeField] private Canvas _baseCanvas;
        [SerializeField] private RectTransform _upgradeViewsContainer;
        [SerializeField] private Button _closeButton;
        [SerializeField] private UpgradeFocusView _upgradeFocusView;
        [SerializeField] private RectTransform _currencyViewContainer;
        [SerializeField] private CurrencyView _currencyView;

        private MenuViewModel _menuViewModel;
        private ViewFactory _viewFactory;
        private UpgradeView[] _upgradeViews;
        private UpgradeViewModel _upgradeViewModel;
        private CurrencyViewModel _currencyViewModel;

        private void OnEnable()
        {
            if (_menuViewModel == null)
                return;

            SubscribeToViewModel();
        }

        private void OnDisable()
        {
            if (_menuViewModel == null)
                return;

            UnsubscribeToViewModel();
        }

        private void SubscribeToViewModel()
        {
            _menuViewModel.OpenedUpgradeSelection += Show;
            _menuViewModel.DisabledUpgradeSelection += Hide;
        }

        private void UnsubscribeToViewModel()
        {
            _menuViewModel.OpenedUpgradeSelection -= Show;
            _menuViewModel.DisabledUpgradeSelection -= Hide;
        }

        public void Initialize(
            MenuViewModel menuViewModel,
            UpgradeViewModel upgradeViewModel,
            CurrencyViewModel currencyViewModel,
            ViewFactory viewFactory,
            UpgradeDescriptionBuilder descriptionBuilder)
        {
            _menuViewModel = menuViewModel;
            _upgradeViewModel = upgradeViewModel;
            _viewFactory = viewFactory;
            _currencyViewModel = currencyViewModel;

            _upgradeFocusView.Initialize(_upgradeViewModel,
                _viewFactory,
                _currencyViewModel,
                descriptionBuilder);
            BindUpgradeViews();

            _currencyView.Initialize(_currencyViewModel, descriptionBuilder);

            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            SubscribeToViewModel();

            Hide();
        }

        private void BindUpgradeViews()
        {
            _upgradeViews = _viewFactory.CreateUpgradeViews();
            foreach (UpgradeView upgradeView in _upgradeViews)
            {
                upgradeView.transform.SetParent(_upgradeViewsContainer);
                upgradeView.transform.localScale = Vector3.one;
            }
        }

        private void OnCloseButtonClicked() => _menuViewModel.DisableUpgradeSelection();

        private void Show() => _baseCanvas.enabled = true;

        private void Hide() => _baseCanvas.enabled = false;
    }
}