using System;

namespace Modules.DAL.Abstract.Data
{
    public interface IEntity: ICloneable
    {
        string Id { get; }
    }
}