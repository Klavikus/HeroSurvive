using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Data;
using Modules.DAL.Abstract.DataContexts;
using Modules.DAL.Abstract.Repositories;

namespace Modules.DAL.Implementation.Repositories
{
    public class CompositeRepository : ICompositeRepository
    {
        private readonly IDataContext _baseDataContext;
        private readonly Dictionary<Type, IRepository> _reposByTypes = new();

        public CompositeRepository(IDataContext baseDataContext, IEnumerable<Type> repoTypes)
        {
            _baseDataContext = baseDataContext;
            HandledTypes = repoTypes.ToList();

            foreach (Type repoType in HandledTypes)
                _reposByTypes.Add(repoType, new Repository(repoType, baseDataContext));
        }

        public IEntity Add<T>(IEntity entity) where T : class, IEntity =>
            _reposByTypes.TryGetValue(typeof(T), out IRepository repository)
                ? repository.Add(entity)
                : null;

        public IEnumerable<Type> HandledTypes { get; }

        public T GetById<T>(string id) where T : class, IEntity
        {
            if (_reposByTypes.TryGetValue(typeof(T), out IRepository repository))
                return repository.GetById(id) as T;

            return null;
        }

        public List<T> GetAll<T>() where T : class, IEntity
        {
            if (_reposByTypes.TryGetValue(typeof(T), out IRepository repository))
                return repository.GetAll().Cast<T>().ToList();

            return null;
        }

        public List<IEntity> GetAll(Type type)
        {
            if (_reposByTypes.TryGetValue(type, out IRepository repository))
                return repository.GetAll();

            return null;
        }

        public UniTask Load() =>
            _baseDataContext.Load();

        public UniTask Save() =>
            _baseDataContext.Save();

        public void CopyFrom(ICompositeRepository compositeRepository)
        {
            foreach (Type type in compositeRepository.HandledTypes)
            {
                _reposByTypes[type].Clear();

                foreach (IEntity entity in compositeRepository.GetAll(type))
                    _reposByTypes[type].Add(entity);
            }
        }

        public void Clear()
        {
            foreach (IRepository repository in _reposByTypes.Values)
                repository.Clear();
        }

        public void Remove<T>(T entity) where T : class, IEntity
        {
            if (_reposByTypes.TryGetValue(typeof(T), out IRepository repository))
                repository.Delete(entity);
        }

        public void Remove<T>(Func<IEntity, bool> predicate) where T : class, IEntity
        {
            if (_reposByTypes.TryGetValue(typeof(T), out IRepository repository))
                repository.Delete(predicate);
        }
    }
}