using CodeBase.GameCore.Infrastructure.Builders;
using CodeBase.GameCore.Infrastructure.Factories;
using CodeBase.GameCore.Presentation.ViewModels;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CodeBase.GameCore.Presentation.Views.Upgrades
{
    public class UpgradesSelectorView : MonoBehaviour
    {
        [SerializeField] private Canvas _baseCanvas;
        [SerializeField] private RectTransform _upgradeViewsContainer;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _userNameButton;
        [SerializeField] private UpgradeFocusView _upgradeFocusView;
        [SerializeField] private RectTransform _currencyViewContainer;
        [SerializeField] private CurrencyView _currencyView;
        [SerializeField] private int _rowCount;
        [SerializeField] private int _colCount;

        private MenuViewModel _menuViewModel;
        private ViewFactory _viewFactory;
        private UpgradeView[] _upgradeViews;
        private UpgradeViewModel _upgradeViewModel;
        private CurrencyViewModel _currencyViewModel;
        private PlayerInputActions _playerInputActions;

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
            _playerInputActions = new PlayerInputActions();

            Hide();
        }

        private void SubscribeToInputActions()
        {
            _playerInputActions.UI.Apply.performed += OnApplyPerformed;
            _playerInputActions.UI.Cancel.performed += OnCancelPerformed;
            _playerInputActions.UI.ScrollLeft.performed += OnScrollLeftPerformed;
            _playerInputActions.UI.ScrollRight.performed += OnScrollRightPerformed;
            _playerInputActions.UI.ScrollUp.performed += OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed += OnScrollDownPerformed;
        }

        private void UnsubscribeToInputActions()
        {
            _playerInputActions.UI.Apply.performed -= OnApplyPerformed;
            _playerInputActions.UI.Cancel.performed -= OnCancelPerformed;
            _playerInputActions.UI.ScrollLeft.performed -= OnScrollLeftPerformed;
            _playerInputActions.UI.ScrollRight.performed -= OnScrollRightPerformed;
            _playerInputActions.UI.ScrollUp.performed -= OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed -= OnScrollDownPerformed;
        }

        private void OnApplyPerformed(InputAction.CallbackContext context) => _upgradeFocusView.OnBuyButtonClicked();

        private void OnCancelPerformed(InputAction.CallbackContext context) => OnCloseButtonClicked();

        private void OnScrollUpPerformed(InputAction.CallbackContext context) =>
            _upgradeViewModel.HandleMove(0, -1, _rowCount, _colCount);

        private void OnScrollDownPerformed(InputAction.CallbackContext context) =>
            _upgradeViewModel.HandleMove(0, 1, _rowCount, _colCount);

        private void OnScrollLeftPerformed(InputAction.CallbackContext context) =>
            _upgradeViewModel.HandleMove(-1, 0, _rowCount, _colCount);

        private void OnScrollRightPerformed(InputAction.CallbackContext context) =>
            _upgradeViewModel.HandleMove(1, 0, _rowCount, _colCount);

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

        private void Show()
        {
            _baseCanvas.enabled = true;
            SubscribeToInputActions();
            _playerInputActions.Enable();
        }

        private void Hide()
        {
            _baseCanvas.enabled = false;
            UnsubscribeToInputActions();
            _playerInputActions.Disable();
        }
    }
}