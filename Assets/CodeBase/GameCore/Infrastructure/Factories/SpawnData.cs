using UnityEngine;

namespace CodeBase.GameCore.Infrastructure.Factories
{
    public struct SpawnData
    {
        public SpawnData(Transform pivotObject, int id, float offset, Vector3 startPosition, Vector3 newDirection)
        {
            Id = id;
            Offset = offset;
            StartPosition = startPosition;
            NewDirection = newDirection;
            PivotObject = pivotObject;
        }

        public Transform PivotObject { get; }
        public int Id { get; }
        public float Offset { get; }
        public Vector3 StartPosition { get; }
        public Vector3 NewDirection { get; }
    }
}