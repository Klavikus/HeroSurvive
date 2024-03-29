using CodeBase.Presentation.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Presentation.Views
{
    public class WinView : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _adsDoubleRewardButton;
        [SerializeField] private TMP_Text _killedCounter;

        private GameLoopViewModel _gameLoopViewModel;

        private void OnDisable()
        {
            _gameLoopViewModel.AllWavesCompleted -= OnWin;
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            _adsDoubleRewardButton.onClick.RemoveListener(OnDoubleRewardButtonClicked);
        }

        public void Initialize(GameLoopViewModel gameLoopViewModel)
        {
            _gameLoopViewModel = gameLoopViewModel;
            _mainCanvas.enabled = false;
            _gameLoopViewModel.AllWavesCompleted += OnWin;
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _adsDoubleRewardButton.onClick.AddListener(OnDoubleRewardButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            //TODO: Move Time.timescale manipulations into service
            Time.timeScale = 1;
            _gameLoopViewModel.CloseLevel();
        }

        private void OnDoubleRewardButtonClicked() => _gameLoopViewModel.CloseLevelDoubleReward();

        private void OnWin()
        {
            Time.timeScale = 0;
            _mainCanvas.enabled = true;
            _killedCounter.text = _gameLoopViewModel.GetKillCount().ToString();
        }
    }
}