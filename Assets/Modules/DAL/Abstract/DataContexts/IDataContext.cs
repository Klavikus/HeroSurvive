using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Data;

namespace Modules.DAL.Abstract.DataContexts
{
    public interface IDataContext
    {
        IEnumerable<Type> ContainedTypes { get; }
        UniTask Load();
        UniTask Save();
        List<IEntity> Set(Type type);
        void Clear();
    }
}