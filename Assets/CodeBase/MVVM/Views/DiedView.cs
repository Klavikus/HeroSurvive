using CodeBase.Infrastructure.Factories;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views
{
    public class DiedView : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Button _closeButton;
        [SerializeField] private TMP_Text _killedCounter;
        [SerializeField] private TMP_Text _waveCounter;
        [SerializeField] private TMP_Text _goldCounter;

        private GameLoopViewModel _gameLoopViewModel;

        private void OnDisable()
        {
            _gameLoopViewModel.PlayerDied -= OnLose;
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
        }

        public void Initialize(GameLoopViewModel gameLoopViewModel)
        {
            _gameLoopViewModel = gameLoopViewModel;
            _mainCanvas.enabled = false;
            _gameLoopViewModel.PlayerDied += OnLose;
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        private void OnCloseButtonClicked()
        {
            Time.timeScale = 1;
            _gameLoopViewModel.CloseLevel();
        }

        private void OnLose()
        {
            Time.timeScale = 0;
            _killedCounter.text = _gameLoopViewModel.GetKillCount().ToString();
            _waveCounter.text = _gameLoopViewModel.GetClearedWaveCount().ToString();
            _mainCanvas.enabled = true;
        }
    }
}