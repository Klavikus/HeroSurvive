using UnityEngine;
using ILogger = Source.Controllers.Api.Services.ILogger;

namespace Source.Controllers.Core.Services
{
    public class DebugLogger : ILogger
    {
        public void Log(string message) =>
            Debug.Log(message);

        public void LogWarning(string message) =>
            Debug.LogWarning(message);

        public void LogException(string message) =>
            Debug.LogError(message);
    }
}