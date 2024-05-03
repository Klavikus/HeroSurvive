using Modules.DAL.Abstract.Data;

namespace Modules.DAL.Tests.Internal
{
    internal class TestEntity2 : IEntity
    {
        public TestEntity2(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public object Clone() =>
            new TestEntity2(Id);

        public bool Equals(IEntity other) =>
            Id == other?.Id;
    }
}