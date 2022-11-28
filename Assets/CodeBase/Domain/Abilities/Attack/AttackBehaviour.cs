using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Domain.Enemies;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Attack
{
    public abstract class AttackBehaviour : IAttackBehaviour
    {
        protected readonly AbilityData AbilityConfig;

        protected int Penetration;
        protected RaycastHit2D[] Results;
        protected Rigidbody2D Rigidbody2D;
        protected List<Damageable> PreviousEnemies = new();
        protected List<Damageable> CurrentEnemies = new();
        protected WaitForSeconds AttackDelayInSeconds;
        protected bool CanRun;

        public event Action PenetrationLimit;
        public event Action EnemyHitted;

        protected void InvokePenetrationLimit() => PenetrationLimit?.Invoke();
        protected void InvokeEnemyHitted() => EnemyHitted?.Invoke();

        protected AttackBehaviour(AbilityData abilityConfig)
        {
            AbilityConfig = abilityConfig;
            Penetration = AbilityConfig.Penetration;
        }

        public void Initialize(Rigidbody2D rigidbody2D)
        {
            Rigidbody2D = rigidbody2D;
            AttackDelayInSeconds = new WaitForSeconds(AbilityConfig.AttackDelay);
            Results = new RaycastHit2D[AbilityConfig.MaxAffectedEnemy];
            Penetration = AbilityConfig.Penetration;
            CanRun = true;
        }

        public virtual IEnumerator Run()
        {
            yield return AttackDelayInSeconds;
        }
    }
}