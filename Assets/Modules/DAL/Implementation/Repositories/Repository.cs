using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Data;
using Modules.DAL.Abstract.DataContexts;
using Modules.DAL.Abstract.Repositories;

namespace Modules.DAL.Implementation.Repositories
{
    public class Repository : IRepository
    {
        private readonly IDataContext _baseDataContext;

        public Repository(Type handledType, IDataContext baseDataContext)
        {
            HandledType = handledType;
            _baseDataContext = baseDataContext;
        }

        public Type HandledType { get; }

        private List<IEntity> Entities =>
            _baseDataContext.Set(HandledType);

        public IEntity GetById(string id) =>
            Entities.FirstOrDefault(entity => entity.Id == id);

        public List<IEntity> GetAll() =>
            Entities;

        public IEntity Add(IEntity entity)
        {
            Entities.Add(entity);

            return entity;
        }

        public void Delete(IEntity entity) =>
            Entities.Remove(entity);

        public void Clear() =>
            Entities.Clear();

        public async UniTask Save() =>
            await _baseDataContext.Save();

        public UniTask Load() =>
            _baseDataContext.Load();

        public void Delete(Func<IEntity, bool> predicate) =>
            Entities.RemoveAll(entity => predicate(entity));
    }
}