using System;

namespace Modules.DAL.Abstract.Data
{
    public interface IEntity: ICloneable, IEquatable<IEntity>
    {
        string Id { get; }
    }
}