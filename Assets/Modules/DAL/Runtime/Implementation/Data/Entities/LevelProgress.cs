using Modules.DAL.Abstract.Data;

namespace Modules.DAL.Implementation.Data.Entities
{
    public class LevelProgress : IEntity
    {
        public int LevelCurrency;
        public int WinCount;
        public int LevelId;
        public bool IsNudeToggled;

        private string _id;

        public LevelProgress(string id)
        {
            _id = id;
        }

        public bool IsCompleted => WinCount > 0;

        public string Id => _id;

        public object Clone()
        {
            return new LevelProgress(Id)
            {
                LevelId = LevelId,
                LevelCurrency = LevelCurrency,
                WinCount = WinCount,
                IsNudeToggled = IsNudeToggled
            };
        }

        public bool Equals(IEntity other) =>
            Id == other?.Id;
    }
}