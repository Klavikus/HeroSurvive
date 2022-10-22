using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private GameObject _hitVfx;

    public event Action Died;

    public void TakeDamage(int damage)
    {
        _hitVfx.SetActive(true);
        _health -= damage;
        if (_health <= 0)
        {
            Destroy(gameObject);
            _health = 0;
            Died?.Invoke();
        }
    }
}