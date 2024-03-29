using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Domain
{
    public abstract class AttackBehaviour : IAttackBehaviour
    {
        public AbilityData AbilityConfig { get; }
        public int Penetration { get; private set; }
        public RaycastHit2D[] Results { get; private set; }
        public Rigidbody2D Rigidbody2D { get; private set; }
        public List<Damageable> PreviousEnemies { get; } = new();
        public List<Damageable> CurrentEnemies { get; } = new();
        public WaitForSeconds AttackDelayInSeconds { get; private set; }

        public event Action PenetrationLimit;
        public event Action<Transform> EnemyHitted;

        public void Initialize(Rigidbody2D rigidbody2D)
        {
            Rigidbody2D = rigidbody2D;
            AttackDelayInSeconds = new WaitForSeconds(AbilityConfig.AttackDelay);
            Results = new RaycastHit2D[AbilityConfig.MaxAffectedEnemy];
            Penetration = AbilityConfig.Penetration;
        }

        public virtual IEnumerator Run()
        {
            yield return AttackDelayInSeconds;
        }

        public void HandlePenetration() =>
            Penetration--;

        protected void InvokePenetrationLimit() =>
            PenetrationLimit?.Invoke();

        protected void InvokeEnemyHitted(Transform enemy) =>
            EnemyHitted?.Invoke(enemy);

        protected AttackBehaviour(AbilityData abilityConfig)
        {
            AbilityConfig = abilityConfig;
            Penetration = AbilityConfig.Penetration;
        }
    }
}