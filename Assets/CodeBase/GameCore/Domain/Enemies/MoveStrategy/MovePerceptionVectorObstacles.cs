using UnityEngine;

namespace CodeBase.GameCore.Domain.Enemies.MoveStrategy
{
    [CreateAssetMenu(menuName = "SO/MoveStrategy/PerceptionVectorObstacles", fileName = "MovePerceptionVectorObstacles",
        order = 51)]
    public sealed class MovePerceptionVectorObstacles : MoveStrategy
    {
        [SerializeField] private LayerMask _whatIsObstacle;
        [SerializeField] private int _sideVectorsCount;

        private readonly Collider2D[] _colliders = new Collider2D[2];
        private readonly RaycastHit2D[] hits = new RaycastHit2D[2];

        public override Vector3 GetMoveVector(Transform origin, Vector3 target, float checkDistance)
        {
            Vector3 directionToTarget = (target - origin.position).normalized;
            Vector3[] perceptionVectors = GetPerceptionVectors(origin, directionToTarget, _sideVectorsCount, checkDistance);
            Vector3 obstacleVector = GetObstacleVector(perceptionVectors);

            if (CheckForwardObstacle(origin, directionToTarget * checkDistance))
                return Vector3.zero;

            Vector3 correctedDirection = (directionToTarget - obstacleVector).normalized;

            return correctedDirection;
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

        private Vector3 GetObstacleVector(Vector3[] perceptionVectors)
        {
            Vector3 result = Vector3.zero;

            foreach (Vector3 perceptionVector in perceptionVectors)
                result += perceptionVector;

            return result.normalized;
        }

        private Vector3[] GetPerceptionVectors(Transform origin, Vector3 directionToTarget, int vectorsCount,
            float checkDistance)
        {
            Vector3[] result = new Vector3[_sideVectorsCount];
            float rotationStep = 360f / vectorsCount;

            for (int i = 0; i < vectorsCount; i++)
            {
                result[i] = Vector3.zero;
                int hitsCount = Physics2D.RaycastNonAlloc(origin.position,
                    Quaternion.Euler(new Vector3(0, 0, rotationStep * i)) * directionToTarget, 
                    results: hits,
                    distance: checkDistance,
                    layerMask: _whatIsObstacle);

                for (var j = 0; j < hitsCount; j++)
                {
                    RaycastHit2D hit2D = hits[j];
                    if (hit2D.transform != origin)
                    {
                        result[i] = Quaternion.Euler(new Vector3(0, 0, rotationStep * i)) * directionToTarget *
                                    hit2D.distance;
                        break;
                    }
                }
            }

            return result;
        }
    }
}