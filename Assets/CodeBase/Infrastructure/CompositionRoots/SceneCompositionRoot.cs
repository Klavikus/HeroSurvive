using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.StateMachine;
using CodeBase.Presentation.ViewModels;
using UnityEngine;

namespace CodeBase.Infrastructure.CompositionRoots
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