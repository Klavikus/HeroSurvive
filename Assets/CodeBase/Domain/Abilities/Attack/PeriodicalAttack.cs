using System;
using System.Collections;
using CodeBase.Domain.Enemies;
using UnityEngine;

namespace CodeBase.Domain.Abilities.Attack
{
    [Serializable]
    public sealed class PeriodicalAttack : AttackBehaviour
    {
        public PeriodicalAttack(AbilityData abilityConfig) : base(abilityConfig)
        {
        }

        public override IEnumerator Run()
        {
            yield return base.Run();

            while (_canRun)
            {
                CheckOverlap();
                yield return _attackDelayInSeconds;
            }
        }

        private void CheckOverlap()
        {
            int count = _rigidbody2D.Cast(Vector2.zero, _abilityConfig.WhatIsEnemy, _results);

            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                    if (_results[i].collider.TryGetComponent(out Damageable enemy))
                        enemy.TakeDamage(_abilityConfig.Damage);
            }
        }
    }
}