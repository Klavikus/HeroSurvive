using System;
using System.Collections.Generic;
using CodeBase.Domain.Data;
using CodeBase.MVVM.Builders;
using CodeBase.MVVM.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
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

        [SerializeField] private TweenTrigger _continueButtonTweenTrigger;
        [SerializeField] private TweenTrigger _reRollButtonTweenTrigger;


        private LevelUpViewModel _levelUpViewModel;
        private Dictionary<AbilityUpgradeView, AbilityUpgradeData> _abilityUpgradeDataByView;
        private AbilityUpgradeData _currentSelectedUpgrade;

        private PlayerInputActions _playerInputActions;
        private DefaultInputActions _defaultInputActions;
        private int _currentSelectedViewId = 0;
        private int _maxId = 2;

        private int _currentActiveButtonId;
        private int _maxButtonId = 1;

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


            _currentLevel.text = "1";
            _currentProgress.fillAmount = 0;

            _playerInputActions = new PlayerInputActions();
            _playerInputActions.UI.ScrollUp.performed += OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed += OnScrollDownPerformed;
            _playerInputActions.UI.Apply.performed += OnApplyPerformed;

            _defaultInputActions = new DefaultInputActions();
            _defaultInputActions.UI.Navigate.performed += NavigateOnPerformed;
        }

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

            _playerInputActions.UI.ScrollUp.performed -= OnScrollUpPerformed;
            _playerInputActions.UI.ScrollDown.performed -= OnScrollDownPerformed;
            _playerInputActions.UI.Apply.performed -= OnApplyPerformed;

            _defaultInputActions.Disable();
            _defaultInputActions.UI.Navigate.performed -= NavigateOnPerformed;
        }

        private void NavigateOnPerformed(InputAction.CallbackContext context)
        {
            if (context.ReadValue<Vector2>().x < 0)
            {
                if (_currentActiveButtonId == 0)
                    return;

                _currentActiveButtonId--;

                ActivateSelectedTween();
            }

            if (context.ReadValue<Vector2>().x > 0)
            {
                if (_currentActiveButtonId == 1)
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
            _levelUpViewModel.Reroll();
        }

        private void OnLevelChanged(int newLevel)
        {
            _currentLevel.text = newLevel.ToString();

            if (_levelUpViewModel.GetAvailableUpgrades().Length == 0)
                return;

            Show();
        }

        private void ShowProgress(float progressPercent) => _currentProgress.fillAmount = progressPercent;

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
            _currentActiveButtonId = 0;
            _currentSelectedUpgrade ??= _abilityUpgradeDataByView[_abilityUpgradeViews[0]];
            _levelUpViewModel.SelectUpgrade(_currentSelectedUpgrade);
            _currentSelectedUpgrade = null;
        }

        private void ActivateSelectedTween()
        {
            if (_currentActiveButtonId == 0)
            {
                _continueButton.Select();
                _continueButtonTweenTrigger.InvokeShow();
                _reRollButtonTweenTrigger.InvokeHide();
            }

            if (_currentActiveButtonId == 1)
            {
                _reRollButton.Select();
                _continueButtonTweenTrigger.InvokeHide();
                _reRollButtonTweenTrigger.InvokeShow();
            }
        }

        private void Show()
        {
            ActivateSelectedTween();

            Time.timeScale = 0f;
            _mainCanvas.enabled = true;

            _currentSelectedViewId = 0;
            _playerInputActions?.Enable();
            _defaultInputActions?.Enable();

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
            _continueButtonTweenTrigger.InvokeShow();
            _reRollButtonTweenTrigger.InvokeHide();

            Time.timeScale = 1f;
            _mainCanvas.enabled = false;
            _playerInputActions?.Disable();
            _defaultInputActions?.Disable();
        }

        public void OnHover(BaseEventData eventData)
        {
            if (gameObject == _continueButton.gameObject)
            {
                _currentActiveButtonId = 0;
                ActivateSelectedTween();
                return;
            }
            if (gameObject == _reRollButton.gameObject)
            {
                _currentActiveButtonId = 1;
                ActivateSelectedTween();
                return;
            }
        }
    }
}