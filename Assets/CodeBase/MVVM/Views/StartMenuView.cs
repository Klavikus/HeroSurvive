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
        private bool _isInitialized;

        public void Initialize(MenuViewModel menuViewModel)
        {
            _menuViewModel = menuViewModel;
            _menuViewModel.InvokedMainMenuShow += Show;
            _menuViewModel.InvokedMainMenuHide += Hide;

            _openHeroSelection.onClick.AddListener(ShowHeroSelection);
            _openUpgradeSelection.onClick.AddListener(ShowUpgradeSelection);
            _openLeaderBoard.onClick.AddListener(ShowLeaderBoard);

            Show();

            _isInitialized = true;
        }

        private void Show()
        {
            _baseCanvas.enabled = true;
        }

        private void Hide()
        {
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
        }

        private void ShowHeroSelection() => _menuViewModel.EnableHeroSelection();
        private void ShowUpgradeSelection() => _menuViewModel.EnableUpgradeSelection();
        private void ShowLeaderBoard() => _menuViewModel.InvokeLeaderBoardShow();
    }
}