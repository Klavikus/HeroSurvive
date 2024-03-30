using Modules.Infrastructure.Implementation.DI;
using UnityEngine;

namespace Modules.Infrastructure.Implementation
{
    public abstract class SceneCompositionRoot : MonoBehaviour
    {
        public abstract void Initialize(ServiceContainer serviceContainer);
    }
}