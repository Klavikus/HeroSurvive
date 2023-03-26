using CodeBase.Infrastructure;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Presentation
{
    public class LeaderBoardViewInvoker : MonoBehaviour
    {
        [SerializeField] private Button _showButton;
        private MenuViewModel _menuViewModel;

        private void Start()
        {
            _menuViewModel = AllServices.Container.AsSingle<IViewModelProvider>().Get<MenuViewModel>();
            _showButton.onClick.AddListener(_menuViewModel.InvokeLeaderBoardShow);
        }

        private void OnDisable() => _showButton.onClick.RemoveListener(_menuViewModel.InvokeLeaderBoardShow);
    }
}