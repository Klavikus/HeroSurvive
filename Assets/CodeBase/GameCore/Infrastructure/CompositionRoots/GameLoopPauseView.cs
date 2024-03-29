using CodeBase.GameCore.Presentation.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GameCore.Infrastructure.CompositionRoots
{
    internal class GameLoopPauseView : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Button _closeButton;

        private GameLoopPauseViewModel _gameLoopPauseViewModel;

        private bool _isInitialized;

        public void Initialize(GameLoopPauseViewModel gameLoopPauseViewModel)
        {
            _gameLoopPauseViewModel = gameLoopPauseViewModel;

            _gameLoopPauseViewModel.InvokedShow += Show;
            _gameLoopPauseViewModel.InvokedHide += Hide;

            _closeButton.onClick.AddListener(OnCloseButtonClicked);

            _isInitialized = true;
        }

        private void OnCloseButtonClicked() => _gameLoopPauseViewModel.InvokeHide();

        private void OnDestroy()
        {
            if (_isInitialized == false)
                return;

            _gameLoopPauseViewModel.InvokedShow -= Show;
            _gameLoopPauseViewModel.InvokedHide -= Hide;
        }

        private void Show() => _mainCanvas.enabled = true;
        private void Hide() => _mainCanvas.enabled = false;
    }
}