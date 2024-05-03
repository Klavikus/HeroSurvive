using Modules.DAL.Abstract.Data;

namespace Modules.DAL.Tests.Internal
{
    internal class TestEntity3 : IEntity
    {
        public string Id { get; set; }

        public object Clone() =>
            new TestEntity3 {Id = Id};

        public bool Equals(IEntity other) =>
            other?.Id == Id;
    }
}