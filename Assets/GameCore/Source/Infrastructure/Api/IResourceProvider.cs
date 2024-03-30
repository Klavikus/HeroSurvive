using UnityEngine;

namespace Source.Infrastructure.Api
{
    public interface IResourceProvider
    {
        T Load<T>() where T : Object;
    }
}