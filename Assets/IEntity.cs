using System;

internal interface IEntity
{
    event Action<EntityState> StateChanged;
}