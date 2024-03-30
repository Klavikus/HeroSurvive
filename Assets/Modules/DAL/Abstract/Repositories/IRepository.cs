using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Data;

namespace Modules.DAL.Abstract.Repositories
{
    public interface IRepository
    {
        IEntity GetById(string id);
        List<IEntity> GetAll();
        IEntity Add(IEntity entity);
        void Delete(IEntity entity);
        void Clear();
        UniTask Save();
        UniTask Load();
        void Delete(Func<IEntity, bool> predicate);
    }
}