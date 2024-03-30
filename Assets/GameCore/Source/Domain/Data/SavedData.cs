using System;

namespace Source.Domain.Data
{
    [Serializable]
    public class SavedData
    {
        public LevelProgress[] LevelsProgress = Array.Empty<LevelProgress>();
    }
}