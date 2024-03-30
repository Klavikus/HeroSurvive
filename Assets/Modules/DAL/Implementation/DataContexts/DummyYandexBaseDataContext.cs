using System;
using Cysharp.Threading.Tasks;
using Modules.DAL.Abstract.Data;
using UnityEngine;

namespace Modules.DAL.Implementation.DataContexts
{
    [Serializable]
    public class DummyYandexBaseDataContext : BaseDataContext
    {
        public DummyYandexBaseDataContext(IData data) : base(data)
        {
        }

        public override UniTask Load()
        {
            UniTaskCompletionSource<string> taskCompletionSource = new UniTaskCompletionSource<string>();

            bool completed = false;

            Debug.LogWarning("StartLoadDummy");
            Test(() =>
            {
                Debug.LogWarning("OnCompleteTest");

                taskCompletionSource.TrySetResult("");
            });

            // taskCompletionSource.TrySetResult("data");

            Debug.LogWarning("EndLoadDummy");

            return taskCompletionSource.Task;
        }

        private async void Test(Action onCompleted)
        {
            Debug.LogWarning("StartTest");

            await UniTask.Delay(TimeSpan.FromSeconds(5f));
            Debug.LogWarning("EndTest");

            onCompleted?.Invoke();
        }

        public override UniTask Save()
        {
            UniTaskCompletionSource<string> taskCompletionSource = new UniTaskCompletionSource<string>();
            taskCompletionSource.TrySetResult(String.Empty);

            return taskCompletionSource.Task;
        }
    }
}