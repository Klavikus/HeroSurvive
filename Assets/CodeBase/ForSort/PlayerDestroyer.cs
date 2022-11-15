using System.Collections;
using CodeBase.Domain.Enemies;
using UnityEngine;

public class PlayerDestroyer : MonoBehaviour
{
    [SerializeField] private ContactFilter2D _layerMask;
    [SerializeField] private int _damage;
    [SerializeField] private float _destroyDelay;
    [SerializeField] private float _checkRadius;

    private readonly RaycastHit2D[] _results = new RaycastHit2D[15];

    private IEnumerator Start()
    {
        while (true)
        {
            int count = Physics2D.CircleCastNonAlloc(transform.position, _checkRadius, Vector2.one, _results, 1,
                _layerMask.layerMask);

            if (count > 0)
                _results[0].collider.GetComponent<IDamageable>().TakeDamage(_damage);

            yield return new WaitForSeconds(_destroyDelay);
        }
    }
}