using CodeBase.MVVM.ViewModels;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.MVVM.Views
{
    public class StartMenuView : MonoBehaviour
    {
        [SerializeField] private Canvas _baseCanvas;
        [SerializeField] private Button _openHeroSelection;
        [SerializeField] private Button _openUpgradeSelection;
        [SerializeField] private Button _openLeaderBoard;
        [SerializeField] private Button _openOptions;

        private MenuViewModel _menuViewModel;

        public void Initialize(MenuViewModel menuViewModel)
        {
            _menuViewModel = menuViewModel;

            _openHeroSelection.onClick.AddListener(ShowHeroSelection);
            _openUpgradeSelection.onClick.AddListener(ShowUpgradeSelection);
            _openLeaderBoard.onClick.AddListener(ShowLeaderBoard);

            _baseCanvas.enabled = true;
        }

        private void OnDisable()
        {
            _openHeroSelection.onClick.RemoveListener(ShowHeroSelection);
            _openUpgradeSelection.onClick.RemoveListener(ShowUpgradeSelection);
            _openLeaderBoard.onClick.RemoveListener(ShowLeaderBoard);
        }

        private void ShowHeroSelection() => _menuViewModel.EnableHeroSelection();
        private void ShowUpgradeSelection() => _menuViewModel.EnableUpgradeSelection();
        private void ShowLeaderBoard() => _menuViewModel.InvokeLeaderBoardShow();
    }
}