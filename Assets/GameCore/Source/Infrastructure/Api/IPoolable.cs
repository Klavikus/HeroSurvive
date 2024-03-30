using System;

namespace Source.Infrastructure.Api
{
    public interface IPoolable<out T>
    {
        event Action<T> ReadyToRelease;

        void Release();
    }
}