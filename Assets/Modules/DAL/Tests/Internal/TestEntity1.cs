using Modules.DAL.Abstract.Data;

namespace Modules.DAL.Tests.Internal
{
    internal class TestEntity1 : IEntity
    {
        public TestEntity1(string id)
        {
            Id = id;
        }

        public string Id { get;  }
        public string Name { get; set; }

        public object Clone() =>
            new TestEntity1(Id);

        public bool Equals(IEntity other) =>
            Id == other?.Id;
    }
}