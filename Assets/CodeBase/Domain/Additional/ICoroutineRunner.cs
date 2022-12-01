using System.Collections;
using UnityEngine;

namespace CodeBase.Domain.Additional
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        Coroutine Run(IEnumerator coroutine);
        void Stop(Coroutine coroutine);
    }
}