using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health;
    [SerializeField] private ParticleSystem _hitVfx;

    public event Action Died;

    public void TakeDamage(int damage)
    {
        _hitVfx.Play();
        _health -= damage;
        if (_health <= 0)
        {
            _health = 0;
            Died?.Invoke();
        }
    }
}