using System;

namespace GameCore.Source.Controllers.Api.Services
{
    public interface ILeveCompetitionService
    {
        event Action<int> WaveCompleted;
        event Action AllWavesCompleted;
        event Action EnemyKilled;
        int KilledEnemiesCount { get; }
        int TotalKilledEnemiesCount { get; }
        int CurrentWaveId { get; }
        int TotalGoldGained { get; }
        void StartCompetition();
        void Stop();
        int GetAllWavesCount();
    }
}