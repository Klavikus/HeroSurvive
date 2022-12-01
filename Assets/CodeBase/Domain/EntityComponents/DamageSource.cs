using CodeBase.Domain.Data;
using CodeBase.Domain.Enemies;
using UnityEngine;

namespace CodeBase.Domain.EntityComponents
{
    public class DamageSource
    {
        private readonly Transform _pivotObject;
        private readonly RaycastHit2D[] _results;
    
        private DamageSourceData _damageSourceData;

        public DamageSource(DamageSourceData damageSourceData, Transform pivotObject)
        {
            _damageSourceData = damageSourceData;
            _pivotObject = pivotObject;
            _results = new RaycastHit2D[_damageSourceData.MaxAffectedTargets];
        }

        public void HandleAttack()
        {
            int availableTargetsCount = CalculateAvailableTargets();

            for (int i = 0; i < availableTargetsCount; i++)
                if (_results[i].transform.TryGetComponent(out IDamageable damageable))
                    damageable.TakeDamage(_damageSourceData.Damage, 0);
        }

        private int CalculateAvailableTargets() =>
            Physics2D.CircleCastNonAlloc(
                origin: _pivotObject.position,
                radius: _damageSourceData.AttackRadius,
                direction: Vector2.zero,
                results: _results,
                distance: 0,
                layerMask: _damageSourceData.WhatIsTarget.layerMask);

        public void UpdateProgression(float stageProgressionModifier) => 
            _damageSourceData.UpdateProgression(stageProgressionModifier);
    }
}