using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Abilities.Attack
{
    public abstract class AttackBehaviour : IAttackBehaviour
    {
        protected readonly AbilityData _abilityConfig;

        protected int Penetration;
        protected RaycastHit2D[] _results;
        protected Rigidbody2D _rigidbody2D;
        protected List<Enemy> _previousEnemys = new List<Enemy>();
        protected List<Enemy> _currentEnemys = new List<Enemy>();
        protected WaitForSeconds _attackDelayInSeconds;
        protected bool _canRun;

        public event Action PenetrationLimit;

        protected void InvokePenetrationLimit()
        {
            PenetrationLimit?.Invoke();
        }

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