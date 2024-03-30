using GameCore.Source.Infrastructure.Core.Services.DI;
using UnityEngine;

namespace GameCore.Source.Application.CompositionRoots
{
    public abstract class SceneCompositionRoot : MonoBehaviour
    {
        public abstract void Initialize(ServiceContainer serviceContainer);
    }
}