using System;
using System.Collections.Generic;

namespace Modules.DAL.Abstract.Data
{
    public interface IData
    {
        IEnumerable<Type> ContainedTypes { get; }
        
        List<IEntity> Set(Type type);
        List<T> InjectWithReplace<T>(List<T> list) where T : class, IEntity;
        void Clear();
        void CopyFrom(Dictionary<Type, List<IEntity>> deserializedData);
    }
}