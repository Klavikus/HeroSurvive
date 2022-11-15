using CodeBase.Infrastructure.Factories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views
{
    public class WinView : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _killedCounter;

        private GameLoopViewModel _gameLoopViewModel;

        private void OnDisable()
        {
            _gameLoopViewModel.AllWavesCompleted -= OnWin;
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            //TODO: Move Time.timescale manipulations into service
            Time.timeScale = 1;
            _gameLoopViewModel.CloseLevel();
        }

        public void Initialize(GameLoopViewModel gameLoopViewModel)
        {
            _gameLoopViewModel = gameLoopViewModel;
            _mainCanvas.enabled = false;
            _gameLoopViewModel.AllWavesCompleted += OnWin;
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnWin()
        {
            Time.timeScale = 0;
            _mainCanvas.enabled = true;
            _killedCounter.text = _gameLoopViewModel.GetKillCount().ToString();
        }
    }
}