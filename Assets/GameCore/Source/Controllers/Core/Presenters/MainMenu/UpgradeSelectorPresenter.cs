using System;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Core.Presenters.Base;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using GameCore.Source.Presentation.Api;
using GameCore.Source.Presentation.Api.Factories;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.Common.WindowFsm.Runtime.Abstract;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCore.Source.Controllers.Core.Presenters.MainMenu
{
    public class UpgradeSelectorPresenter : BaseWindowPresenter<UpgradeSelectorWindow>
    {
        private readonly IUpgradesSelectorView _view;
        private readonly IPersistentUpgradeService _persistentUpgradeService;
        private readonly IPersistentUpgradeViewFactory _persistentUpgradeViewFactory;
        private readonly CurrencyModel _currencyModel;
        private readonly PlayerInputActions _playerInputActions;
        private readonly int _rowCount;
        private readonly int _colCount;

        public UpgradeSelectorPresenter(IWindowFsm windowFsm,
            IUpgradesSelectorView view,
            IPersistentUpgradeService persistentUpgradeService,
            IPersistentUpgradeViewFactory persistentUpgradeViewFactory)
            : base(windowFsm, view.Canvas)
        {
            _view = view;
            _persistentUpgradeService = persistentUpgradeService ??
                                        throw new ArgumentNullException(nameof(persistentUpgradeService));
            _persistentUpgradeViewFactory = persistentUpgradeViewFactory ??
                                            throw new ArgumentNullException(nameof(persistentUpgradeViewFactory));

            _playerInputActions = new PlayerInputActions();

            _rowCount = view.RowCount;
            _colCount = view.ColCount;
        }

        protected override void OnAfterEnable()
        {
            foreach (IPersistentUpgradeView upgradeView in _persistentUpgradeViewFactory.Create())
            {
                upgradeView.Transform.SetParent(_view.UpgradeViewsContainer);
                upgradeView.Transform.localScale = Vector3.one;
            }

            _view.Initialize();

            _view.CloseButton.Clicked += Close;

            _persistentUpgradeService.UpgradeSelected += OnUpgradeSelected;
        }

        protected override void OnAfterDisable()
        {
            _view.CloseButton.Clicked -= Close;
            _persistentUpgradeService.UpgradeSelected -= OnUpgradeSelected;
            UnsubscribeToInputActions();
        }

        protected override void OnAfterOpened() =>
            SubscribeToInputActions();

        protected override void OnAfterClosed() =>
            UnsubscribeToInputActions();

        private void SubscribeToInputActions()
        {
            _playerInputActions.UI.Apply.performed += OnApplyPerformed;
            _playerInputActions.UI.Cancel.performed += OnCancelPerformed;
            _playerInputActions.UI.ScrollLeft.performed += OnScrollLeftPerformed;
            _playerInputActions.UI.ScrollRight.performed += OnScrollRightPerformed;
            _playerInputActions.UI.ScrollUp.performed += OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed += OnScrollDownPerformed;
            _playerInputActions.Enable();
        }

        private void UnsubscribeToInputActions()
        {
            _playerInputActions.UI.Apply.performed -= OnApplyPerformed;
            _playerInputActions.UI.Cancel.performed -= OnCancelPerformed;
            _playerInputActions.UI.ScrollLeft.performed -= OnScrollLeftPerformed;
            _playerInputActions.UI.ScrollRight.performed -= OnScrollRightPerformed;
            _playerInputActions.UI.ScrollUp.performed -= OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed -= OnScrollDownPerformed;
            _playerInputActions.Disable();
        }

        private void OnApplyPerformed(InputAction.CallbackContext context) =>
            BuyUpgrade();

        private void OnCancelPerformed(InputAction.CallbackContext context) =>
            Close();

        private void OnScrollUpPerformed(InputAction.CallbackContext context) =>
            _persistentUpgradeService.HandleMove(0, -1, _rowCount, _colCount);

        private void OnScrollDownPerformed(InputAction.CallbackContext context) =>
            _persistentUpgradeService.HandleMove(0, 1, _rowCount, _colCount);

        private void OnScrollLeftPerformed(InputAction.CallbackContext context) =>
            _persistentUpgradeService.HandleMove(-1, 0, _rowCount, _colCount);

        private void OnScrollRightPerformed(InputAction.CallbackContext context) =>
            _persistentUpgradeService.HandleMove(1, 0, _rowCount, _colCount);

        private void BuyUpgrade()
        {
        }

        private void Close()
        {
            WindowFsm.Close<UpgradeSelectorWindow>();
        }

        // public void BindUpgradeViews()
        // {
        //     _upgradeViews = _viewFactory.CreateUpgradeViews();
        //
        //     foreach (IUpgradeView upgradeView in _upgradeViews)
        //     {
        //         upgradeView.transform.SetParent(_upgradeViewsContainer);
        //         upgradeView.transform.localScale = Vector3.one;
        //     }
        // }
        private void OnUpgradeSelected(UpgradeData upgradeData, int level)
        {
            Debug.Log($"{upgradeData.KeyName} {level}");
        }
    }
}