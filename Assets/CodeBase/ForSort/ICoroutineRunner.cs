using System.Collections;
using UnityEngine;

namespace CodeBase.ForSort
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        Coroutine Run(IEnumerator coroutine);
        // T InstantiateGameObject<T>(T prefab, Vector2 position, Quaternion rotation, bool isSelfParent) where T : Object;
        void Stop(Coroutine coroutine);
    }
}