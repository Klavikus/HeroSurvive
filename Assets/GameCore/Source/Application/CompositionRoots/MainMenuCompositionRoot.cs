using Source.Infrastructure.Core;
using Source.Infrastructure.Core.Services.DI;
using UnityEngine;

namespace Source.Application.CompositionRoots
{
    public class MainMenuCompositionRoot : SceneCompositionRoot
    {
        [SerializeField] private ConfigurationContainer _configurationContainer;

        private void Start() =>
            Initialize(new ServiceContainer());

        public override async void Initialize(ServiceContainer serviceContainer)
        {

        }
    }
}