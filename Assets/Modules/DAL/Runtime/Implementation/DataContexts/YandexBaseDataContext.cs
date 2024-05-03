#if YANDEX
using System;
using Agava.YandexGames;
using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Data;
using Modules.DAL.Abstract.DataContexts;
using Modules.DAL.Abstract.Serialization;
using Modules.DAL.Implementation.Serialization;

namespace Modules.DAL.Implementation.DataContexts
{
    [Serializable]
    public class YandexBaseDataContext : BaseDataContext
    {
        private const string StorageKey = "YandexData";

        private IJsonSerializer _jsonSerializer;

        public YandexBaseDataContext(IData data) : base(data)
        {
            _jsonSerializer = new JsonSerializer(data.ContainedTypes.Concat(new[] {typeof(GameDataDto)}));
        }

        public override UniTask Load()
        {
            UniTaskCompletionSource<string> taskCompletionSource = new UniTaskCompletionSource<string>();

            if (Bridge.storage.IsAvailable(StorageType.PlatformInternal) == false)
            {
                taskCompletionSource.TrySetResult(string.Empty);

                return taskCompletionSource.Task;
            }

            Bridge.storage.Get(StorageKey, (isCompleted, data) =>
            {
                if (isCompleted)
                {
                    Debug.Log("deserializedData");

                    data ??= string.Empty;

                    GameDataDto deserializedData = _jsonSerializer.Deserialize<GameDataDto>(data);
                    Debug.Log("deserializedData loaded");

                    if (deserializedData.Equals(default(GameDataDto)) == false && deserializedData.Data != null)
                    {
                        Debug.Log("GameDataDto not default");
                        Data.CopyFrom(deserializedData.Data);
                    }

                    taskCompletionSource.TrySetResult(data);
                }
                else
                {
                    taskCompletionSource.TrySetException(new Exception("Cloud save data is null"));
                }
            });

            return taskCompletionSource.Task;
        }

        public override UniTask Save()
        {
            string resultJsonString = _jsonSerializer.Serialize(GameDataDto.FromGameData(Data));

            Debug.Log("save to cloud" + resultJsonString);

            UniTaskCompletionSource<string> taskCompletionSource = new UniTaskCompletionSource<string>();

            if (Bridge.storage.IsAvailable(StorageType.PlatformInternal) == false)
            {
                taskCompletionSource.TrySetException(new Exception("Platform error!"));

                return taskCompletionSource.Task;
            }

            Bridge.storage.Set(StorageKey, resultJsonString, (isCompleted) =>
            {
                if (isCompleted)
                {
                    Debug.Log("Cloud save completed!");
                    taskCompletionSource.TrySetResult(default);
                }
                else
                {
                    taskCompletionSource.TrySetException(new Exception("Cloud save not completed"));
                }
            });

            return taskCompletionSource.Task;
        }
    }
}
#endif