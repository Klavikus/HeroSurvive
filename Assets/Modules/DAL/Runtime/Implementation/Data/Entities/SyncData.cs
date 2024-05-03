using Modules.DAL.Abstract.Data;

namespace Modules.DAL.Implementation.Data.Entities
{
    public class SyncData : IEntity
    {
        public uint SyncCount;

        public SyncData()
        {
            Id = nameof(SyncData);
        }

        public string Id { get; }

        public object Clone() =>
            new SyncData() {SyncCount = SyncCount};
        
        public bool Equals(IEntity other) =>
            Id == other?.Id;
    }
}