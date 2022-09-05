using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private LayerMask _whatIsEnemy;
    [SerializeField] private int _speed;
    [SerializeField] private int _damage;
    [SerializeField] private int _maxAffectedEnemy;
    [SerializeField] private float _radius;
    [SerializeField] private float _lifeTime;

    private RaycastHit2D[] _results;
    private List<Enemy> _previousEnemys = new List<Enemy>();
    private List<Enemy> _currentEnemys = new List<Enemy>();

    private void Start()
    {
        _results = new RaycastHit2D[_maxAffectedEnemy];
        Destroy(gameObject, _lifeTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * (_speed * Time.deltaTime));
        // CheckOverlap();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(_damage);
        }
    }

    private void CheckOverlap()
    {
        int count = Physics2D.CircleCastNonAlloc(transform.position, _radius, Vector2.zero, _results, 1, _whatIsEnemy);

        if (count > 0)
        {
            for (var i = 0; i < count; i++)
                if (_results[i].collider.TryGetComponent(out Enemy enemy))
                    _currentEnemys.Add(enemy);

            foreach (Enemy newEnemy in _currentEnemys.Except(_previousEnemys))
                newEnemy.TakeDamage(_damage);
        }

        _previousEnemys.Clear();
        _previousEnemys.AddRange(_currentEnemys);
        _currentEnemys.Clear();
    }
}