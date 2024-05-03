using System;

namespace Modules.DAL.Tests.Internal
{
    internal class TestInvalidEntity
    {
        public TestInvalidEntity()
        {
            InvalidId = nameof(InvalidId);
            InvalidName = nameof(InvalidName);
        }

        public string InvalidId { get; }
        public string InvalidName { get; }

        public override bool Equals(object obj) =>
            (obj as TestInvalidEntity)?.InvalidId == InvalidId;

        protected bool Equals(TestInvalidEntity other) =>
            InvalidId == other.InvalidId && InvalidName == other.InvalidName;

        public override int GetHashCode() =>
            HashCode.Combine(InvalidId, InvalidName);
    }
}