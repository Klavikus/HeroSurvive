using System;
using CodeBase.GameCore.Infrastructure.Services;
using UnityEngine.InputSystem;

namespace CodeBase.GameCore.Presentation.ViewModels
{
    public class GameLoopPauseViewModel
    {
        private readonly IGamePauseService _gamePauseService;
        private readonly PlayerInputActions _playerInputActions;

        public GameLoopPauseViewModel(IGamePauseService gamePauseService)
        {
            _gamePauseService = gamePauseService;
            _playerInputActions = new PlayerInputActions();
            _gamePauseService.PauseStarted += InvokeShow;
            _playerInputActions.UI.Cancel.performed += OnCancelPerformed;
            _playerInputActions.Enable();
        }

        ~GameLoopPauseViewModel()
        {
            _gamePauseService.PauseStarted -= InvokeShow;
            _playerInputActions.UI.Cancel.performed -= OnCancelPerformed;
            _playerInputActions.Disable();
        }

        private void OnCancelPerformed(InputAction.CallbackContext ctx)
        {
            if (_gamePauseService.IsPaused)
                InvokeHide();
            else
                InvokeShow();
        }

        public event Action InvokedShow;
        public event Action InvokedHide;

        public void InvokeShow()
        {
            InvokedShow?.Invoke();
            _gamePauseService.StartPauseByMenu();
        }

        public void InvokeHide()
        {
            InvokedHide?.Invoke();
            _gamePauseService.StopPauseByRaceMenu();
        }
    }
}