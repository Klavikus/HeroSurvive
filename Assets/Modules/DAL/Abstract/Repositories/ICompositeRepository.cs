using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Data;

namespace Modules.DAL.Abstract.Repositories
{
    public interface ICompositeRepository
    {
        IEnumerable<Type> HandledTypes { get; }

        T GetById<T>(string id) where T : class, IEntity;
        IEntity Add<T>(IEntity entity) where T : class, IEntity;
        List<T> GetAll<T>() where T : class, IEntity;
        List<IEntity> GetAll(Type type);
        UniTask Load();
        UniTask Save();
        void CopyFrom(ICompositeRepository compositeRepository);
        void Clear();
        void Remove<T>(T entity) where T : class, IEntity;
        void Remove<T>(Func<IEntity, bool> predicate) where T : class, IEntity;
    }
}