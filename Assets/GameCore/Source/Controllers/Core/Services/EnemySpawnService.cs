using System;
using System.Collections.Generic;
using System.Linq;
using GameCore.Source.Controllers.Api;
using GameCore.Source.Controllers.Core.Factories;
using GameCore.Source.Domain.Data;
using GameCore.Source.Domain.Models;
using GameCore.Source.Domain.Services;
using Modules.Common.Utils;
using UnityEngine;
using IEnemySpawnService = GameCore.Source.Controllers.Api.Services.IEnemySpawnService;

namespace GameCore.Source.Controllers.Core.Services
{
    public class EnemySpawnService : IEnemySpawnService
    {
        private const float CellSize = 1;
        private const int CellFactor = 3;
        private readonly ITargetService _targetFinderService;
        private readonly EnemyFactory _enemyFactory;

        public EnemySpawnService(ITargetService targetFinderService, EnemyFactory enemyFactory)
        {
            _targetFinderService = targetFinderService;
            _enemyFactory = enemyFactory;
        }

        public IEnemyController[] SpawnWave(EnemySpawnData[] enemiesSpawnData)
        {
            List<IEnemyController> result = new List<IEnemyController>();
            List<Vector2> points = CalculateAvailableSpawnPoints(CellSize, CellFactor * enemiesSpawnData.Sum(data => data.Count),
                _targetFinderService.GetCamera(), _targetFinderService.GetPlayerPosition());
            points.Shuffle();

            foreach (EnemySpawnData spawnData in enemiesSpawnData)
            {
                Vector2[] subset = points.Take(spawnData.Count).ToArray();
                
                for (int i = 0; i < spawnData.Count; i++)
                {
                    Vector3 spawnPosition = subset[i];
                    IEnemyController enemy = _enemyFactory.Create(at: spawnPosition, spawnData.EnemyType, _targetFinderService);
                    result.Add(enemy);
                }

                points = points.Except(subset).ToList();
            }

            return result.ToArray();
        }

        public void ClearEnemies()
        {
            _enemyFactory.ClearEnemies();
        }

        private List<Vector2> CalculateAvailableSpawnPoints(float cellSizeX, int minCellCount, Camera mainCamera,
            Vector2 pivotPosition)
        {
            Vector2 topRightCorner = new Vector2(1, 1);
            Vector2 bottomLeftCorner = new Vector2(0, 0);
            Vector2 topEdgeVector = mainCamera.ViewportToWorldPoint(topRightCorner);
            Vector2 bottomEdgeVector = mainCamera.ViewportToWorldPoint(bottomLeftCorner);
            float height = topEdgeVector.y - bottomEdgeVector.y;
            float width = topEdgeVector.x - bottomEdgeVector.x;

            int cellsPerZone = Mathf.CeilToInt(minCellCount / 4f);
            int foundedCells = 0;

            RectangleSpawnZone[] rectangleSpawnZones =
                new RectangleSpawnZone[Enum.GetValues(typeof(RectangleSpawnZone.SpawnZoneDirection)).Length];
            Rectangle restricted = new Rectangle(pivotPosition.x - width / 2,
                pivotPosition.y + height / 2, width, height);

            for (int i = 0; i < Enum.GetValues(typeof(RectangleSpawnZone.SpawnZoneDirection)).Length; i++)
            {
                rectangleSpawnZones[i] = new RectangleSpawnZone(
                    (RectangleSpawnZone.SpawnZoneDirection) i,
                    pivotPosition,
                    restricted,
                    cellSizeX,
                    cellsPerZone,
                    GameConstants.SpawnRectangleRestriction);

                if (rectangleSpawnZones[i].IsLocked == false)
                    foundedCells += cellsPerZone;
            }

            if (foundedCells < minCellCount)
                rectangleSpawnZones
                    .First(zone => zone.IsLocked == false)
                    .RecalculateCells(cellsPerZone + (minCellCount - foundedCells));

            List<Vector2> availableSpawnPoints = new List<Vector2>();

            foreach (var spawnZone in rectangleSpawnZones)
            {
                if (spawnZone.IsLocked)
                    continue;

                availableSpawnPoints.AddRange(spawnZone.GetAvailableGridPoints());
            }

            return availableSpawnPoints;
        }
    }
}