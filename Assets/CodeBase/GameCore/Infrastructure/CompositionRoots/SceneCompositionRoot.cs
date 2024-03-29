using CodeBase.GameCore.Infrastructure.Services;
using CodeBase.GameCore.Infrastructure.StateMachine;
using CodeBase.GameCore.Presentation.ViewModels;
using UnityEngine;

namespace CodeBase.GameCore.Infrastructure.CompositionRoots
{
    public class SceneCompositionRoot : MonoBehaviour
    {
        [SerializeField] private GameLoopPauseView _gameLoopPauseView;

        public void Initialize(AllServices container)
        {
            _gameLoopPauseView.Initialize(container.AsSingle<IViewModelProvider>().Get<GameLoopPauseViewModel>());
        }
    }
}