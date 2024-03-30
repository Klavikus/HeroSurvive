namespace GameCore.Source.Controllers.Api.Services
{
    public interface ILogger
    {
        void Log(string message);
        void LogWarning(string message);
        void LogException(string message);
    }
}