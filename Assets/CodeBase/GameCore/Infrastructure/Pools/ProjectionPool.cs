using System.Collections.Generic;
using System.Linq;
using CodeBase.GameCore.Domain.Abilities;
using UnityEngine;

namespace CodeBase.GameCore.Infrastructure.Pools
{
    public class ProjectionPool
    {
        private readonly Transform _container;
        private readonly AbilityProjection _abilityProjection;
        private readonly List<AbilityProjection> _projections;

        private bool _isLocked;
        
        public ProjectionPool(Transform container, AbilityProjection abilityProjection)
        {
            _container = container;
            _abilityProjection = abilityProjection;
            _projections = new List<AbilityProjection>();
        }

        public AbilityProjection[] GetProjections(int count)
        {
            if (_isLocked)
                return null;
            
            List<AbilityProjection> result = _projections
                .Where(x => x.gameObject.activeSelf == false)
                .Take(count)
                .ToList();

            int forSpawn = count - result.Count;

            if (forSpawn > 0)
            {
                for (int i = 0; i <= forSpawn; i++)
                {
                    AbilityProjection projection = Object.Instantiate(_abilityProjection, Vector3.zero,
                        Quaternion.identity, _container);
                    _projections.Add(projection);
                    projection.Destroed += OnProjectionDestroyed;
                    result.Add(projection);
                }
            }

            return result.ToArray();
        }

        public void Clear()
        {
            _isLocked = true;
            foreach (AbilityProjection projection in _projections)
                if (projection)
                    Object.Destroy(projection.gameObject);
            _projections.Clear();
        }

        private void OnProjectionDestroyed(AbilityProjection abilityProjection) => 
            _projections.Remove(abilityProjection);
    }
}