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
        private IJsonSerializer _jsonSerializer;

        public YandexBaseDataContext(IData data) : base(data)
        {
            _jsonSerializer = new JsonSerializer(data.ContainedTypes);
        }

        public override UniTask Load()
        {
            UniTaskCompletionSource<string> taskCompletionSource = new UniTaskCompletionSource<string>();

            PlayerAccount.GetCloudSaveData((data) =>
                {
                    if (data != null)
                    {
                        GameDataDto deserializedData = _jsonSerializer.Deserialize<GameDataDto>(data);

                        Data.CopyFrom(deserializedData.Data);

                        taskCompletionSource.TrySetResult(data);
                    }
                    else
                    {
                        taskCompletionSource.TrySetException(new Exception("Cloud save data is null"));
                    }
                },
                error => taskCompletionSource.TrySetException(new Exception(error)));

            return taskCompletionSource.Task;
        }
        
        public override UniTask Save()
        {
            string resultJsonString = _jsonSerializer.Serialize(GameDataDto.FromGameData(Data));
            
            UniTaskCompletionSource<string> taskCompletionSource = new UniTaskCompletionSource<string>();
           
            PlayerAccount.SetCloudSaveData(
                resultJsonString,
                onSuccessCallback: () => taskCompletionSource.TrySetResult(String.Empty),
                onErrorCallback: error => taskCompletionSource.TrySetException(new Exception(error))
            );

            return taskCompletionSource.Task;
        }
    }
}
#endif