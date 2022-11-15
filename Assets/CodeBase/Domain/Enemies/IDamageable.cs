﻿using System;
using CodeBase.Domain.Data;

namespace CodeBase.Domain.Enemies
{
    public interface IDamageable
    {
        event Action<int, float> HealthChanged;
        event Action<int> DamageTaken;
        event Action<int> HealTaken;
        event Action Died;
        void Initialize(DamageableData damageableData);
        void TakeDamage(int damage);
        void RestoreHealth(int healAmount);
    }
}