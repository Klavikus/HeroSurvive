using UnityEngine;

namespace CodeBase.SO.MoveStrategy
{
    [CreateAssetMenu(menuName = "SO/MoveStrategy/OnlyForwardObstacle", fileName = "MoveOnlyForwardObstacle", order = 51)]
    public sealed class MoveOnlyForwardObstacle : MoveStrategy
    {
        [SerializeField] private LayerMask _whatIsObstacle;

        private readonly Collider2D[] _colliders = new Collider2D[2];

        public override Vector3 GetMoveVector(Transform origin, Vector3 target, float checkDistance)
        {
            Vector3 directionToTarget = (target - origin.position).normalized;

            if (CheckForwardObstacle(origin, directionToTarget* checkDistance))
                return Vector3.zero;

            return directionToTarget;
        }

        private bool CheckForwardObstacle(Transform origin, Vector3 directionToTarget)
        {
            int hitsCount =
                Physics2D.OverlapPointNonAlloc(origin.position + directionToTarget, _colliders, _whatIsObstacle);

            if (hitsCount > 0)
                for (var i = 0; i < hitsCount; i++)
                    if (_colliders[i].transform != origin)
                        return true;

            return false;
        }
    }
}