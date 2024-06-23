using System;

namespace Modules.Common.Utils
{
    public interface IMultiCallHandler
    {
        event Action Called;
        event Action Released;
        bool IsCalled { get; }
        void Call(string key);
        void Release(string key);
        void Reset();
        bool CheckCallStatus(string key);
    }
}