using System;
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

        private Dictionary<IAbilityUpgradeView, AbilityUpgradeData> _abilityUpgradeDataByView;

        private int _respawnsCount;

        private int _currentSelectedViewId = 0;
        private int _maxId = 2;
        private int _currentActiveButtonId;
        private int _maxButtonId = 1;
        private AbilityUpgradeData _currentSelectedUpgrade;

        public LevelUpSystemPresenter(
            IWindowFsm windowFsm,
            ILevelUpSystemView view,
            ILevelUpViewModel viewModel,
            IGamePauseService gamePauseService,
            ILocalizationService localizationService,
            IUpgradeDescriptionBuilder upgradeDescriptionBuilder)
            : base(windowFsm, view.Canvas)
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
                // upgradeView.Initialize(upgradeDescriptionBuilder);
                _abilityUpgradeDataByView.Add(upgradeView, null);
                upgradeView.Selected += OnUpgradeSelected;
            }
        }

        protected override void OnAfterDisable()
        {
            _viewModel.LevelChanged -= OnLevelChanged;
            _viewModel.AvailableUpgradesChanged -= ShowAvailableUpgrades;
            _viewModel.Rerolled -= Show;

            _view.ContinueButton.Clicked -= OnContinueButtonClicked;
            _view.ReRollButton.Clicked -= OnReRollButtonClicked;

            _viewModel.ResetLevels();
        }

        protected override void OnAfterOpened()
        {
            _gamePauseService.InvokeByUI(true);

            _playerInputActions.UI.ScrollUp.performed += OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed += OnScrollDownPerformed;
            _playerInputActions.UI.Apply.performed += OnApplyPerformed;

            _defaultInputActions.UI.Navigate.performed += NavigateOnPerformed;

            _playerInputActions.Enable();
            _defaultInputActions.Enable();

            Show();
        }

        protected override void OnAfterClosed()
        {
            _playerInputActions.UI.ScrollUp.performed -= OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed -= OnScrollDownPerformed;
            _playerInputActions.UI.Apply.performed -= OnApplyPerformed;

            _defaultInputActions.UI.Navigate.performed -= NavigateOnPerformed;

            _playerInputActions.Disable();
            _defaultInputActions.Disable();

            _gamePauseService.InvokeByUI(false);
        }

        private void NavigateOnPerformed(InputAction.CallbackContext context)
        {
            if (context.ReadValue<Vector2>().x < 0)
            {
                if (context.control.IsPressed() == false || _currentActiveButtonId == 0)
                    return;

                _currentActiveButtonId--;

                ActivateSelectedTween();
            }

            if (context.ReadValue<Vector2>().x > 0)
            {
                if (context.control.IsPressed() == false || _currentActiveButtonId == 1)
                    return;

                _currentActiveButtonId++;

                ActivateSelectedTween();
            }
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
            _currentActiveButtonId = 1;
            _viewModel.Reroll();
        }

        private void OnLevelChanged(int newLevel)
        {
            // _currentLevel.text = newLevel.ToString();

            if (_viewModel.GetAvailableUpgrades().Length == 0)
                return;

            WindowFsm.OpenWindow<LevelUpWindow>();
        }

        private void ShowAvailableUpgrades(AbilityUpgradeData[] abilityUpgradesData)
        {
            for (int i = 0; i < _abilityUpgradeViews.Length; i++)
            {
                FillUpgradeView(_abilityUpgradeViews[i], abilityUpgradesData[i]);

                // _abilityUpgradeViews[i].Show(abilityUpgradesData[i]);
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
                // _continueButtonTweenTrigger.InvokeShow();
                // _reRollButtonTweenTrigger.InvokeHide();
            }

            if (_currentActiveButtonId == 1)
            {
                // _continueButtonTweenTrigger.InvokeHide();
                // _reRollButtonTweenTrigger.InvokeShow();
            }
        }

        private void Show()
        {
            _currentSelectedViewId = 0;

            ActivateSelectedTween();

            AbilityUpgradeData[] upgradesData = _viewModel.GetAvailableUpgrades();

            for (int i = 0; i < _abilityUpgradeViews.Length; i++)
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
    }
}