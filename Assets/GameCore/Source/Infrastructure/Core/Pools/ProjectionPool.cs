using System.Collections.Generic;
using System.Linq;
using GameCore.Source.Infrastructure.Api;
using GameCore.Source.Presentation.Api.GameLoop.Abilities;
using UnityEngine;

namespace GameCore.Source.Infrastructure.Core.Pools
{
    public class ProjectionPool : IProjectionPool
    {
        private readonly Transform _container;
        private readonly GameObject _abilityProjection;
        private readonly List<IAbilityProjection> _projections;

        private bool _isLocked;

        public ProjectionPool(Transform container, GameObject abilityProjection)
        {
            _container = container;
            _abilityProjection = abilityProjection;
            _projections = new List<IAbilityProjection>();
        }

        public IAbilityProjection[] GetProjections(int count)
        {
            if (_isLocked)
                return null;

            List<IAbilityProjection> result = _projections
                .Where(x => x.GameObject.activeSelf == false)
                .Take(count)
                .ToList();

            int forSpawn = count - result.Count;

            if (forSpawn > 0)
            {
                for (int i = 0; i < forSpawn; i++)
                {
                    GameObject projectionObject = Object.Instantiate(_abilityProjection, Vector3.zero,
                        Quaternion.identity, _container);
                    IAbilityProjection projection = projectionObject.GetComponent<IAbilityProjection>();
                    
                    _projections.Add(projection);
                    projection.Destroyed += OnProjectionDestroyed;
                    result.Add(projection);
                }
            }

            return result.ToArray();
        }

        public void Clear()
        {
            _isLocked = true;
            foreach (IAbilityProjection projection in _projections)
                if (projection.GameObject)
                    Object.Destroy(projection.GameObject);
            _projections.Clear();
        }

        private void OnProjectionDestroyed(IAbilityProjection abilityProjection) =>
            _projections.Remove(abilityProjection);
    }
}