using System;
using CodeBase.Domain.Data;

namespace CodeBase.ForSort
{
    public interface IInputHandler
    {
        public event Action<InputData> InputUpdated;
    }
}