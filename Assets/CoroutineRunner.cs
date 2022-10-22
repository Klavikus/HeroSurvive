using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
{
    public void Run(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public T InstantiateGameObject<T>(T prefab, Vector2 position, Quaternion rotation, bool isSelfParent)
        where T : Object
    {
        if (isSelfParent)
            return Instantiate(prefab, position, rotation, transform);

        return Instantiate(prefab, position, rotation);
    }

    public T InstantiateGameObject<T>(T prefab, Vector2 position, Quaternion rotation, Transform parent)
        where T : Object
    {
        Debug.Log($"instantiate");
        return Instantiate(prefab, position, rotation, parent);
    }
}