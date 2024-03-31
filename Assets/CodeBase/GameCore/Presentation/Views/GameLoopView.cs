using CodeBase.GameCore.Infrastructure.Builders;
using CodeBase.GameCore.Presentation.ViewModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.GameCore.Presentation.Views
{
    public class GameLoopView : MonoBehaviour
    {
        [SerializeField] private Button _closeLevelButton;
        [SerializeField] private Slider _wavesSlider;
        [SerializeField] private TMP_Text _wavesCounter;
        [SerializeField] private LevelUpView _levelUpView;
        [SerializeField] private CounterView _killCounterView;
        [SerializeField] private CounterView _currencyCounterView;
        [SerializeField] private WinView _winView;
        [SerializeField] private DiedView _diedView;

        private GameLoopViewModel _gameLoopViewModel;

        private void OnDisable()
        {
            _gameLoopViewModel.KilledChanged -= _killCounterView.OnCounterChanged;
            _gameLoopViewModel.RewardCurrencyChanged -= _currencyCounterView.OnCounterChanged;
            _gameLoopViewModel.WaveCompleted -= OnWaveCompleted;
        }

        public void Initialize(
            GameLoopViewModel gameLoopViewModel,
            LevelUpViewModel levelUpViewModel,
            UpgradeDescriptionBuilder upgradeDescriptionBuilder)
        {
            _gameLoopViewModel = gameLoopViewModel;
            _levelUpView.Initialize(levelUpViewModel, upgradeDescriptionBuilder);
            _killCounterView.Initialize();
            _currencyCounterView.Initialize();
            _gameLoopViewModel.KilledChanged += _killCounterView.OnCounterChanged;
            _gameLoopViewModel.RewardCurrencyChanged += _currencyCounterView.OnCounterChanged;
            _winView.Initialize(gameLoopViewModel);
            _diedView.Initialize(gameLoopViewModel);
            _closeLevelButton.onClick.AddListener(OnCloseLevelButtonClicked);
            _gameLoopViewModel.WaveCompleted += OnWaveCompleted;
            _wavesSlider.maxValue = _gameLoopViewModel.GetAllWavesCount();
            //TODO: Change to TextBuilder
            _wavesSlider.value = 0;
            _wavesCounter.text = "1";
        }

        private void OnWaveCompleted(int completedWaveIndex)
        {
            _wavesSlider.value = completedWaveIndex;
            _wavesCounter.text = completedWaveIndex.ToString();
        }

        private void OnCloseLevelButtonClicked() => _gameLoopViewModel.CloseLevel();
    }
}