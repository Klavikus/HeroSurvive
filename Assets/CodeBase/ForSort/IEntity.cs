using System;

namespace CodeBase.ForSort
{
    internal interface IEntity
    {
        event Action<EntityState> StateChanged;
    }
}