using System;
using Source.Domain.Data;

namespace Source.Domain
{
    public class Level
    {
        public Level(int id, StageVariantData[] stageVariantsData)
        {
            Id = id;
            StagesCompletionStatus = new bool[stageVariantsData.Length];
        }

        public event Action<Level> Started;
        public event Action<Level> Ended;

        public int Id { get; private set; }
        public bool[] StagesCompletionStatus { get; private set; }

        public void CompleteStage(int stageId)
        {
            StagesCompletionStatus[stageId] = true;
        }

        public void ApplyLevelProgress(LevelProgress levelProgress)
        {
            StagesCompletionStatus = levelProgress.StagesCompletionStatus;
        }
    }
}