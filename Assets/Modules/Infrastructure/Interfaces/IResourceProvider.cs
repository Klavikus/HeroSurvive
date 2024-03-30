using UnityEngine;

namespace Modules.Infrastructure.Interfaces
{
    public interface IResourceProvider
    {
        T Load<T>() where T : Object;
    }
}