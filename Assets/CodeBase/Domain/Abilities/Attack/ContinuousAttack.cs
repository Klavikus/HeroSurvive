using System;
using System.Collections;
using System.Linq;
using CodeBase.Domain.Enemies;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Attack
{
    [Serializable]
    public sealed class ContinuousAttack : AttackBehaviour
    {
        public ContinuousAttack(AbilityData abilityConfig) : base(abilityConfig)
        {
        }

        public override IEnumerator Run()
        {
            yield return base.Run();

            while (_canRun)
            {
                CheckOverlap();
                yield return null;
            }
        }

        private void CheckOverlap()
        {
            int count = _rigidbody2D.Cast(Vector2.zero, _abilityConfig.WhatIsEnemy, _results);

            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                    if (_results[i].collider.TryGetComponent(out Damageable damageable) && damageable.CanReceiveDamage)
                        _currentEnemys.Add(damageable);

                foreach (Damageable newDamageable in _currentEnemys.Except(_previousEnemys))
                {
                    newDamageable.TakeDamage(_abilityConfig.Damage, _abilityConfig.Stagger);
                    if (_abilityConfig.IsLimitedPenetration && --Penetration == 0)
                    {
                        _canRun = false;
                        InvokePenetrationLimit();
                    }
                }

                if (_currentEnemys.Except(_previousEnemys).Any()) 
                    InvokeEnemyHitted();
            }

            _previousEnemys.Clear();
            _previousEnemys.AddRange(_currentEnemys);
            _currentEnemys.Clear();
        }


    }
}