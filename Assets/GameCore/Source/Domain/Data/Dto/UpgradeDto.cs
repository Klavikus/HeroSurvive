using System;
using Modules.DAL.Abstract.Data;

namespace GameCore.Source.Domain.Data.Dto
{
    [Serializable]
    public class UpgradeDto : IEntity
    {
        public int Level;

        public UpgradeDto(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public object Clone()
        {
            return new UpgradeDto(Id)
            {
                Level = Level
            };
        }

        public bool Equals(IEntity other) =>
            other?.Id == Id;
    }
}