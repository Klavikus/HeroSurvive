using System;

namespace CodeBase.GameCore.Domain.EntityComponents
{
    public interface IDamageable
    {
        event Action<int, float> HealthChanged;
        event Action<int, float> DamageTaken;
        event Action<int> HealTaken;
        event Action Died;
        void TakeDamage(int damage, float stagger);
        void RestoreHealth(int healAmount);
        void Respawn();
    }
}