using System;
using GameCore.Source.Domain.Data;

namespace GameCore.Source.Domain.EntityComponents
{
    public interface IDamageable
    {
        event Action<int, float> HealthChanged;
        event Action<int, float> DamageTaken;
        event Action<int> HealTaken;
        event Action Died;
        float CurrentHealthPercent { get; }
        void Initialize(DamageableData damageableData);
        void TakeDamage(int damage, float stagger);
        void RestoreHealth(int healAmount);
        void Respawn();
    }
}