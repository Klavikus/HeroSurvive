using System.Collections;
using UnityEngine;

namespace CodeBase.ForSort
{
    public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        public Coroutine Run(IEnumerator coroutine) => StartCoroutine(coroutine);

        public void Stop(Coroutine coroutine)
        {
            throw new System.NotImplementedException();
        }
    }
}