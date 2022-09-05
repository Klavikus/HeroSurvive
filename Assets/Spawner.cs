using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _delay;

    private IEnumerator Start()
    {
        Physics2D.reuseCollisionCallbacks = true;
        var delay = new WaitForSeconds(_delay);

        while (true)
        {
            Instantiate(_prefab, transform.position, Quaternion.identity);
            yield return delay;
        }
    }
}