using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Data;
using Modules.DAL.Abstract.DataContexts;
using Modules.DAL.Abstract.Serialization;
using Modules.DAL.Implementation.Serialization;
using UnityEngine;

namespace Modules.DAL.Implementation.DataContexts
{
    public class JsonPrefsDataContext : BaseDataContext
    {
        private readonly string _key;
        private readonly IJsonSerializer _jsonSerializer;

        public JsonPrefsDataContext(IData data, string key) : base(data)
        {
            _key = key;
            _jsonSerializer = new JsonSerializer(data.ContainedTypes.Concat(new[] {typeof(GameDataDto)}));
        }

        public override async UniTask Load()
        {
            string json = PlayerPrefs.GetString(_key, String.Empty);

            if (string.IsNullOrEmpty(json))
                return;

            GameDataDto deserializedData = _jsonSerializer.Deserialize<GameDataDto>(json);

            Data.CopyFrom(deserializedData.Data);

            await UniTask.Yield();
        }

        public override async UniTask Save()
        {
            string json = _jsonSerializer.Serialize(GameDataDto.FromGameData(Data));

            PlayerPrefs.SetString(_key, json);
            PlayerPrefs.Save();

            await UniTask.Yield();
        }
    }
}