using System;
using System.Collections;
using System.Collections.Generic;
using CodeBase.Domain.Enemies;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Attack
{
    public abstract class AttackBehaviour : IAttackBehaviour
    {
        protected readonly AbilityData _abilityConfig;

        protected int Penetration;
        protected RaycastHit2D[] _results;
        protected Rigidbody2D _rigidbody2D;
        protected List<Damageable> _previousEnemys = new List<Damageable>();
        protected List<Damageable> _currentEnemys = new List<Damageable>();
        protected WaitForSeconds _attackDelayInSeconds;
        protected bool _canRun;

        public event Action PenetrationLimit;
        public event Action EnemyHitted;

        protected void InvokePenetrationLimit() => PenetrationLimit?.Invoke();
        protected void InvokeEnemyHitted() => EnemyHitted?.Invoke();

        protected AttackBehaviour(AbilityData abilityConfig)
        {
            _abilityConfig = abilityConfig;
            Penetration = _abilityConfig.Penetration;
        }

        public void Initialize(Rigidbody2D rigidbody2D)
        {
            _rigidbody2D = rigidbody2D;
            _attackDelayInSeconds = new WaitForSeconds(_abilityConfig.AttackDelay);
            _results = new RaycastHit2D[_abilityConfig.MaxAffectedEnemy];
            Penetration = _abilityConfig.Penetration;
            _canRun = true;
        }

        public virtual IEnumerator Run()
        {
            yield return _attackDelayInSeconds;
        }
    }
}