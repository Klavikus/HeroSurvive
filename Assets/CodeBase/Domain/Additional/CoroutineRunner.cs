using System.Collections;
using UnityEngine;

namespace CodeBase.Domain
{
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        public Coroutine Run(IEnumerator coroutine) => StartCoroutine(coroutine);
        public void Stop(Coroutine coroutine) => StopCoroutine(coroutine);
    }
}