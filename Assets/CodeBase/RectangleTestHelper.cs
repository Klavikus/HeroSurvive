using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Configs;
using NaughtyAttributes;
using UnityEngine;

namespace CodeBase
{
    public class RectangleTestHelper : MonoBehaviour
    {
        [SerializeField] private float _cellSizeX;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _minCellCount;
        [SerializeField] private Camera _camera;
        [SerializeField] private Transform _pivotTransform;

        [Button(nameof(Spawn))]
        private List<Vector2> Spawn()
        {
            return AvailableSpawnPoints(_cellSizeX, _minCellCount, _camera, _pivotTransform);
        }

        private List<Vector2> AvailableSpawnPoints(float cellSizeX, int minCellCount, Camera mainCamera, Transform pivotTransform)
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
            Rectangle restricted = new Rectangle(pivotTransform.position.x - width / 2,
                pivotTransform.position.y + height / 2, width, height);

            for (int i = 0; i < Enum.GetValues(typeof(RectangleSpawnZone.SpawnZoneDirection)).Length; i++)
            {
                rectangleSpawnZones[i] = new RectangleSpawnZone(
                    (RectangleSpawnZone.SpawnZoneDirection) i,
                    pivotTransform.position,
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