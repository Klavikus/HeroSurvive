using System;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public class LevelProgress
    {
        public bool[] StagesCompletionStatus;
        public int Id;

        public static LevelProgress FromModel(Level level)
        {
            return new LevelProgress()
            {
                Id = level.Id,
                StagesCompletionStatus = level.StagesCompletionStatus
            };
        }
    }
}