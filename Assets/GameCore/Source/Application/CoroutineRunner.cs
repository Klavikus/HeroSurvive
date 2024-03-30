using GameCore.Source.Infrastructure.Api.Services;
using UnityEngine;

namespace GameCore.Source.Application
{
    internal class CoroutineRunner : MonoBehaviour, ICoroutineRunner
    {
        private void Awake() => 
            DontDestroyOnLoad(this);
    }
}