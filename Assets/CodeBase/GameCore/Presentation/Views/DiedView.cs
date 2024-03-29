using CodeBase.GameCore.Presentation.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GameCore.Presentation.Views
{
    public class DiedView : MonoBehaviour
    {
        [SerializeField] private Canvas _mainCanvas;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _adsResurrectButton;
        [SerializeField] private Button _adsDoubleGoldButton;
        [SerializeField] private TMP_Text _killedCounter;
        [SerializeField] private TMP_Text _goldCounter;

        private GameLoopViewModel _gameLoopViewModel;

        private void OnDisable()
        {
            _gameLoopViewModel.PlayerDied -= OnLose;
            _gameLoopViewModel.PlayerResurrected -= OnResurrected;
            _closeButton.onClick.RemoveListener(OnCloseButtonClicked);
            _adsResurrectButton.onClick.RemoveListener(OnAdsResurrectButtonClicked);
            _adsDoubleGoldButton.onClick.RemoveListener(OnDoubleGoldButtonClicked);
        }

        public void Initialize(GameLoopViewModel gameLoopViewModel)
        {
            _gameLoopViewModel = gameLoopViewModel;
            _mainCanvas.enabled = false;
            _adsResurrectButton.gameObject.SetActive(true);
            _adsDoubleGoldButton.gameObject.SetActive(false);
            _gameLoopViewModel.PlayerDied += OnLose;
            _gameLoopViewModel.PlayerResurrected += OnResurrected;
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
            _adsResurrectButton.onClick.AddListener(OnAdsResurrectButtonClicked);
            _adsDoubleGoldButton.onClick.AddListener(OnDoubleGoldButtonClicked);
        }

        private void OnDoubleGoldButtonClicked()
        {
            _adsDoubleGoldButton.gameObject.SetActive(false);
            _gameLoopViewModel.CloseLevelDoubleReward();
        }

        private void OnResurrected()
        {
            Time.timeScale = 1;
            _mainCanvas.enabled = false;
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
            _goldCounter.text = _gameLoopViewModel.GetClearedWaveCount().ToString();
            _mainCanvas.enabled = true;
        }

        private void OnAdsResurrectButtonClicked()
        {
            _gameLoopViewModel.ResurrectByAds();
            _adsResurrectButton.gameObject.SetActive(false);
            _adsDoubleGoldButton.gameObject.SetActive(true);
        }
    }
}