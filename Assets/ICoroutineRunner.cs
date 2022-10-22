using System.Collections;
using UnityEngine;

public interface ICoroutineRunner
{
    Coroutine StartCoroutine(IEnumerator coroutine);
    void Run(IEnumerator coroutine);
    T InstantiateGameObject<T>(T prefab, Vector2 position, Quaternion rotation, bool isSelfParent) where T : Object;
}