﻿using System;
using System.Collections.Generic;
using GameCore.Source.Controllers.Api.Factories;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Controllers.Api.ViewModels;
using GameCore.Source.Controllers.Core.Presenters.Base;
using GameCore.Source.Controllers.Core.WindowFsms.Windows;
using GameCore.Source.Domain.Data;
using GameCore.Source.Infrastructure.Api.GameFsm;
using GameCore.Source.Presentation.Api.GameLoop;
using Modules.Common.WindowFsm.Runtime.Abstract;
using Modules.GamePauseSystem.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace GameCore.Source.Controllers.Core.Presenters.GameLoop
{
    public class LevelUpSystemPresenter : BaseWindowPresenter<LevelUpWindow>
    {
        private readonly ILevelUpSystemView _view;
        private readonly ILevelUpViewModel _viewModel;
        private readonly IGamePauseService _gamePauseService;
        private readonly ILocalizationService _localizationService;
        private readonly IUpgradeDescriptionBuilder _upgradeDescriptionBuilder;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IGameLoopService _gameLoopService;
        private readonly PlayerInputActions _playerInputActions;
        private readonly DefaultInputActions _defaultInputActions;
        private readonly IAbilityUpgradeView[] _abilityUpgradeViews;
        private readonly Dictionary<IAbilityUpgradeView, AbilityUpgradeData> _abilityUpgradeDataByView;

        private int _respawnsCount;

        private int _currentSelectedViewId;
        private int _maxId;
        private int _currentActiveButtonId;
        private AbilityUpgradeData _currentSelectedUpgrade;

        public LevelUpSystemPresenter(
            IWindowFsm windowFsm,
            ILevelUpSystemView view,
            ILevelUpViewModel viewModel,
            IGamePauseService gamePauseService,
            ILocalizationService localizationService,
            IUpgradeDescriptionBuilder upgradeDescriptionBuilder)
            : base(windowFsm, view.Show, view.Hide)
        {
            _view = view;
            _viewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
            _gamePauseService = gamePauseService ?? throw new ArgumentNullException(nameof(gamePauseService));
            _localizationService = localizationService;
            _upgradeDescriptionBuilder = upgradeDescriptionBuilder;

            _playerInputActions = new PlayerInputActions();
            _defaultInputActions = new DefaultInputActions();

            _abilityUpgradeViews = _view.AbilityUpgradeViews;
            _abilityUpgradeDataByView = new Dictionary<IAbilityUpgradeView, AbilityUpgradeData>();
        }

        protected override void OnAfterEnable()
        {
            _view.Initialize();

            _viewModel.LevelChanged += OnLevelChanged;
            _viewModel.AvailableUpgradesChanged += ShowAvailableUpgrades;
            _viewModel.Rerolled += Show;

            _view.ContinueButton.Clicked += OnContinueButtonClicked;
            _view.ReRollButton.Clicked += OnReRollButtonClicked;

            foreach (IAbilityUpgradeView upgradeView in _abilityUpgradeViews)
            {
                _abilityUpgradeDataByView.Add(upgradeView, null);
                upgradeView.Selected += OnUpgradeSelected;
            }

            _view.ContinueButton.SetInteractionLock(false);
        }

        protected override void OnAfterDisable()
        {
            _viewModel.LevelChanged -= OnLevelChanged;
            _viewModel.AvailableUpgradesChanged -= ShowAvailableUpgrades;
            _viewModel.Rerolled -= Show;

            _view.ContinueButton.Clicked -= OnContinueButtonClicked;
            _view.ReRollButton.Clicked -= OnReRollButtonClicked;

            foreach (IAbilityUpgradeView upgradeView in _abilityUpgradeViews)
                upgradeView.Selected -= OnUpgradeSelected;

            UnsubscribeFromInputActions();

            _viewModel.ResetLevels();
        }

        protected override void OnAfterOpened()
        {
            _gamePauseService.InvokeByUI(true);

            Show();

            SubscribeToInputActions();
        }

        protected override void OnAfterClosed()
        {
            UnsubscribeFromInputActions();

            _view.ContinueButton.Unfocus();
            _view.ReRollButton.Unfocus();
            
            _gamePauseService.InvokeByUI(false);
        }

        private void OnScrollLeftPerformed(InputAction.CallbackContext context)
        {
            if (context.control.IsPressed() == false || _currentActiveButtonId == 0)
                return;

            _currentActiveButtonId--;

            ActivateSelectedTween();
        }

        private void OnScrollRightPerformed(InputAction.CallbackContext context)
        {
            if (context.control.IsPressed() == false || _currentActiveButtonId == 1)
                return;

            _currentActiveButtonId++;

            ActivateSelectedTween();
        }

        private void OnScrollUpPerformed(InputAction.CallbackContext context)
        {
            if (--_currentSelectedViewId < 0)
                _currentSelectedViewId = _maxId;

            _abilityUpgradeViews[_currentSelectedViewId].OnPointerClick(new PointerEventData(EventSystem.current));
        }

        private void OnScrollDownPerformed(InputAction.CallbackContext context)
        {
            if (++_currentSelectedViewId == _maxId + 1)
                _currentSelectedViewId = 0;

            _abilityUpgradeViews[_currentSelectedViewId].OnPointerClick(new PointerEventData(EventSystem.current));
        }

        private void OnApplyPerformed(InputAction.CallbackContext context)
        {
            if (_currentActiveButtonId == 0)
                OnContinueButtonClicked();
            else
                OnReRollButtonClicked();
        }

        private void OnReRollButtonClicked()
        {
            _viewModel.Reroll();
            _currentActiveButtonId = 0;
            ActivateSelectedTween();
        }

        private void OnLevelChanged(int newLevel)
        {
            if (_viewModel.GetAvailableUpgrades().Length == 0)
                return;

            WindowFsm.OpenWindow<LevelUpWindow>();
        }

        private void ShowAvailableUpgrades(AbilityUpgradeData[] abilityUpgradesData)
        {
            for (int i = 0; i < _abilityUpgradeViews.Length; i++)
            {
                FillUpgradeView(_abilityUpgradeViews[i], abilityUpgradesData[i]);

                _abilityUpgradeDataByView[_abilityUpgradeViews[i]] = abilityUpgradesData[i];
            }
        }

        private void FillUpgradeView(IAbilityUpgradeView view, AbilityUpgradeData data)
        {
            string title = _localizationService.GetLocalizedText(data.BaseConfigSO.UpgradeViewData.TranslatableName);
            string description = _upgradeDescriptionBuilder.GetAbilityUpgradeDescription(data);
            view.Show(data.BaseConfigSO.UpgradeViewData.Icon, title, description);
        }

        private void OnUpgradeSelected(IAbilityUpgradeView selectedUpgrade)
        {
            _currentSelectedUpgrade = _abilityUpgradeDataByView[selectedUpgrade];

            foreach (IAbilityUpgradeView abilityUpgradeView in _abilityUpgradeViews)
                abilityUpgradeView.SetSelected(abilityUpgradeView == selectedUpgrade);
        }

        private void OnContinueButtonClicked()
        {
            _currentActiveButtonId = 0;
            _currentSelectedUpgrade ??= _abilityUpgradeDataByView[_abilityUpgradeViews[0]];
            _viewModel.SelectUpgrade(_currentSelectedUpgrade);
            _currentSelectedUpgrade = null;

            WindowFsm.Close<LevelUpWindow>();
        }

        private void ActivateSelectedTween()
        {
            if (_currentActiveButtonId == 0)
            {
                _view.ContinueButton.Focus();
                _view.ReRollButton.Unfocus();
            }

            if (_currentActiveButtonId == 1)
            {
                _view.ContinueButton.Unfocus();
                _view.ReRollButton.Focus();
            }
        }

        private void Show()
        {
            _currentSelectedViewId = 0;

            ActivateSelectedTween();

            AbilityUpgradeData[] upgradesData = _viewModel.GetAvailableUpgrades();

            int minLength = Mathf.Min(upgradesData.Length, _abilityUpgradeViews.Length);
            _maxId = minLength - 1;

            for (int i = 0; i < minLength; i++)
            {
                if (i < upgradesData.Length)
                {
                    _abilityUpgradeDataByView[_abilityUpgradeViews[i]] = upgradesData[i];
                    FillUpgradeView(_abilityUpgradeViews[i], upgradesData[i]);
                }
                else
                {
                    _abilityUpgradeDataByView[_abilityUpgradeViews[i]] = null;
                    _abilityUpgradeViews[i].Hide();
                }
            }

            _abilityUpgradeViews[0].SetSelected(true);
        }

        private void SubscribeToInputActions()
        {
            _playerInputActions.UI.ScrollUp.performed += OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed += OnScrollDownPerformed;
            _playerInputActions.UI.Apply.performed += OnApplyPerformed;

            _playerInputActions.UI.ScrollLeft.performed += OnScrollLeftPerformed;
            _playerInputActions.UI.ScrollRight.performed += OnScrollRightPerformed;

            _playerInputActions.Enable();
            _defaultInputActions.Enable();
        }

        private void UnsubscribeFromInputActions()
        {
            _playerInputActions.UI.ScrollUp.performed -= OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed -= OnScrollDownPerformed;
            _playerInputActions.UI.Apply.performed -= OnApplyPerformed;

            _playerInputActions.UI.ScrollLeft.performed += OnScrollLeftPerformed;
            _playerInputActions.UI.ScrollRight.performed -= OnScrollRightPerformed;

            _playerInputActions.Disable();
            _defaultInputActions.Disable();
        }
    }
}