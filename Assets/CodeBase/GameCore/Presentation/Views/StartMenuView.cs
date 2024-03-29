using CodeBase.GameCore.Presentation.ViewComponents;
using CodeBase.GameCore.Presentation.ViewModels;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace CodeBase.GameCore.Presentation.Views
{
    public class StartMenuView : MonoBehaviour
    {
        [SerializeField] private Canvas _baseCanvas;
        [SerializeField] private Button _openHeroSelection;
        [SerializeField] private Button _openUpgradeSelection;
        [SerializeField] private Button _openLeaderBoard;
        [SerializeField] private Button _openSettings;
        [SerializeField] private Button _openOptions;
        [SerializeField] private TweenTrigger[] _tweenTriggers;

        private MenuViewModel _menuViewModel;
        private bool _isInitialized;
        private int _currentSelectedTweenerId;
        private DefaultInputActions _defaultInputActions;
        private PlayerInputActions _playerInputActions;

        public void Initialize(MenuViewModel menuViewModel)
        {
            _menuViewModel = menuViewModel;
            _menuViewModel.InvokedMainMenuShow += Show;
            _menuViewModel.InvokedMainMenuHide += Hide;

            _openHeroSelection.onClick.AddListener(ShowHeroSelection);
            _openUpgradeSelection.onClick.AddListener(ShowUpgradeSelection);
            _openLeaderBoard.onClick.AddListener(ShowLeaderBoard);
            _openSettings.onClick.AddListener(ShowSettings);

            foreach (TweenTrigger tweenTrigger in _tweenTriggers)
                tweenTrigger.Showed += OnTweenShowed;

            _defaultInputActions = new DefaultInputActions();
            _defaultInputActions.UI.Navigate.performed += OnNavigationPerformed;
            _defaultInputActions.UI.Submit.performed += OnSubmitPerformed;

            _playerInputActions = new PlayerInputActions();
            _playerInputActions.UI.Apply.performed += OnSubmitPerformed;

            Show();

            _isInitialized = true;
        }

        private void OnSubmitPerformed(InputAction.CallbackContext context)
        {
            switch (_currentSelectedTweenerId)
            {
                case 0:
                    ShowHeroSelection();
                    break;
                case 1:
                    ShowUpgradeSelection();
                    break;
                case 2:
                    ShowLeaderBoard();
                    break;               
                case 3:
                    ShowSettings();
                    break;
            }
        }

        private void OnNavigationPerformed(InputAction.CallbackContext context)
        {
            if (context.control.IsPressed() == false)
                return;

            if (context.ReadValue<Vector2>().x > 0 || context.ReadValue<Vector2>().y < 0)
            {
                if (++_currentSelectedTweenerId == _tweenTriggers.Length)
                    _currentSelectedTweenerId = 0;
            }

            if (context.ReadValue<Vector2>().x < 0 || context.ReadValue<Vector2>().y > 0)
            {
                if (--_currentSelectedTweenerId < 0)
                    _currentSelectedTweenerId = _tweenTriggers.Length - 1;
            }

            ShowCurrentTween();
        }

        private void OnTweenShowed(ITweenTrigger showedTweenTrigger)
        {
            for (var i = 0; i < _tweenTriggers.Length; i++)
            {
                if (_tweenTriggers[i] == showedTweenTrigger)
                {
                    _currentSelectedTweenerId = i;
                    continue;
                }

                _tweenTriggers[i].InvokeHide();
            }
        }

        private void Show()
        {
            _defaultInputActions.Enable();
            _playerInputActions.Enable();
            ShowCurrentTween();
            _baseCanvas.enabled = true;
        }

        private void ShowCurrentTween()
        {
            for (int i = 0; i < _tweenTriggers.Length; i++)
                if (i == _currentSelectedTweenerId)
                    _tweenTriggers[i].InvokeShow();
                else
                    _tweenTriggers[i].InvokeHide();
        }

        private void Hide()
        {
            _defaultInputActions.Disable();
            _playerInputActions.Disable();
            _baseCanvas.enabled = false;
        }

        private void OnDestroy()
        {
            if (_isInitialized == false)
                return;

            _isInitialized = false;

            _openHeroSelection.onClick.RemoveListener(ShowHeroSelection);
            _openUpgradeSelection.onClick.RemoveListener(ShowUpgradeSelection);
            _openLeaderBoard.onClick.RemoveListener(ShowLeaderBoard);

            _menuViewModel.InvokedMainMenuShow -= Show;
            _menuViewModel.InvokedMainMenuHide -= Hide;

            foreach (TweenTrigger tweenTrigger in _tweenTriggers)
                tweenTrigger.Showed -= OnTweenShowed;

            _defaultInputActions.UI.Navigate.performed -= OnNavigationPerformed;
            _defaultInputActions.UI.Submit.performed -= OnSubmitPerformed;
            _playerInputActions.UI.Apply.performed -= OnSubmitPerformed;
        }

        private void ShowHeroSelection() => _menuViewModel.EnableHeroSelection();

        private void ShowUpgradeSelection() => _menuViewModel.EnableUpgradeSelection();

        private void ShowLeaderBoard() => _menuViewModel.InvokeLeaderBoardShow();

        private void ShowSettings() => _menuViewModel.InvokeSettingsShow();
    }
}