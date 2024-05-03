using System;
using System.Collections.Generic;
using System.Linq;
using Modules.DAL.Abstract.Data;

namespace Modules.DAL.Implementation.Data
{
    public class GameData : IData
    {
        private readonly Dictionary<Type, List<IEntity>> _progressByType = new();

        public GameData(IEnumerable<Type> dataTypes)
        {
            if (dataTypes == null)
                throw new ArgumentNullException(nameof(dataTypes));

            Type[] array = dataTypes.ToArray();

            if (array.Length == 0)
                throw new ArgumentException(nameof(dataTypes));

            foreach (Type dataType in array)
            {
                if (typeof(IEntity).IsAssignableFrom(dataType) == false)
                    throw new InvalidCastException($"Type {nameof(dataType)} not implement {nameof(IEntity)}");

                _progressByType.Add(dataType, new List<IEntity>());
            }
        }

        public IEnumerable<Type> ContainedTypes => _progressByType.Keys.ToList();

        public List<IEntity> Set(Type type)
        {
            if (_progressByType.ContainsKey(type) == false)
                throw new ArgumentException(nameof(type));

            return _progressByType[type];
        }

        public List<T> InjectWithReplace<T>(List<T> list) where T : class, IEntity
        {
            if (list == null)
                throw new NullReferenceException(nameof(list));

            _progressByType[typeof(T)].Clear();

            foreach (T entity in list)
                _progressByType[typeof(T)].Add(entity.Clone() as T);

            return _progressByType[typeof(T)].Cast<T>().ToList();
        }

        public void CopyFrom(Dictionary<Type, List<IEntity>> deserializedData)
        {
            foreach (List<IEntity> entities in _progressByType.Values)
                entities.Clear();

            foreach (Type containedType in ContainedTypes)
            {
                if (deserializedData.TryGetValue(containedType, out List<IEntity> entities))
                    _progressByType[containedType] = entities;
            }
        }

        public void Clear()
        {
            foreach (List<IEntity> progress in _progressByType.Values)
                progress.Clear();
        }
    }
}