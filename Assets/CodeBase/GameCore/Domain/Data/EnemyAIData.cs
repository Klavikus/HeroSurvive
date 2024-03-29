using System;
using CodeBase.GameCore.Domain.Enemies.MoveStrategy;

namespace CodeBase.GameCore.Domain.Data
{
    [Serializable]
    public struct EnemyAIData
    {
        //TODO: Refactor this
        public MoveStrategy MoveStrategy;
        public float Speed;
        public float AttackRange;
        public float AttackCheckInterval;
        public float ObstacleCheckDistance;
        public float StaggerResist;

        public void CalculateWithProgression(float progressionModifier) =>
            Speed *= progressionModifier;
    }
}