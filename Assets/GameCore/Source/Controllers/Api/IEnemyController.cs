using System;
using GameCore.Source.Controllers.Api.Services;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Enums;
using GameCore.Source.Domain.Services;
using UnityEngine;

namespace GameCore.Source.Controllers.Api
{
    public interface IEnemyController
    {
        event Action<IEnemyController> Died;
        event Action<IEnemyController> Destroyed;
        event Action<IEnemyController> OutOfViewTimeout;
        event Action<IEnemyController> InvokedBackToPool;
        bool CanReceiveDamage { get; }
        int KillExperience { get; }
        int KillCurrency { get; }
        EnemyType Type { get; }
        Transform Transform { get; }
        void Destroy();

        void Initialize(ITargetService targetFinderService, EnemyData enemyData, IVfxService vfxService,
            IAudioPlayerService audioPlayerService, IGameLoopService gameLoopService);

        void UpdateProgression(float currentPercentage);
    }
}