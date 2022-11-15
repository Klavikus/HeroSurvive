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
        [SerializeField] private Button _openCollections;
        [SerializeField] private Button _openOptions;

        private MenuViewModel _menuViewModel;

        public void Initialize(MenuViewModel menuViewModel)
        {
            _menuViewModel = menuViewModel;

            _openHeroSelection.onClick.AddListener(ShowHeroSelection);
            _openUpgradeSelection.onClick.AddListener(ShowUpgradeSelection);

            _baseCanvas.enabled = true;
        }

        private void ShowHeroSelection() => _menuViewModel.EnableHeroSelection();

        private void ShowUpgradeSelection() => _menuViewModel.EnableUpgradeSelection();

    }
}