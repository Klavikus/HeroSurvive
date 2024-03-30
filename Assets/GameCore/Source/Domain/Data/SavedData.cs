using System;

namespace GameCore.Source.Domain.Data
{
    [Serializable]
    public class SavedData
    {
        public LevelProgress[] LevelsProgress = Array.Empty<LevelProgress>();
    }
}