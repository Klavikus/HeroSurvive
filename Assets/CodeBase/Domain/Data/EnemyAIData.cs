using System;
using CodeBase.SO.MoveStrategy;

namespace CodeBase.Domain
{
    [Serializable]
    public struct EnemyAIData
    {
        public MoveStrategy MoveStrategy;
        public float Speed;
        public float AttackRange;
        public float AttackCheckInterval;
        public float ObstacleCheckDistance;
        public float StaggerResist;

        public void CalculateWithProgression(float progressionModifier) => Speed *= progressionModifier;
    }
}