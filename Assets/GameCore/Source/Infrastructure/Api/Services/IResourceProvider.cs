using UnityEngine;

namespace GameCore.Source.Infrastructure.Api.Services
{
    public interface IResourceProvider
    {
        T Load<T>() where T : Object;
    }
}