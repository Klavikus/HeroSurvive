using System.Collections;
using UnityEngine;

namespace GameCore.Source.Infrastructure.Api.Services
{
    public interface ICoroutineRunner
    {
        Coroutine StartCoroutine(IEnumerator coroutine);
        void StopCoroutine(Coroutine coroutine);
    }
}