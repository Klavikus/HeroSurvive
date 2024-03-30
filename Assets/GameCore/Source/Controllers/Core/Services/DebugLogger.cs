using UnityEngine;

namespace GameCore.Source.Controllers.Core.Services
{
    public class DebugLogger : Api.Services.ILogger
    {
        public void Log(string message) =>
            Debug.Log(message);

        public void LogWarning(string message) =>
            Debug.LogWarning(message);

        public void LogException(string message) =>
            Debug.LogError(message);
    }
}