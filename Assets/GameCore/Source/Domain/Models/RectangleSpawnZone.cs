using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore.Source.Domain.Models
{
    public class RectangleSpawnZone
    {
        public enum SpawnZoneDirection
        {
            I,
            II,
            III,
            IV,
        }

        private Rectangle _mainZone;
        private Rectangle _lockedZone;

        private readonly SpawnZoneDirection _spawnZoneDirection;
        private float _width;
        private float _height;
        private readonly Vector2 _centralPoint;
        private readonly float _cellSize;
        private readonly Vector4 _availableCoordinatesRange;
        private readonly Vector2 _centralPointRes;
        private Vector2 _topLeftCornerCoordinates;


        public RectangleSpawnZone(
            SpawnZoneDirection spawnZoneDirection,
            Vector2 centralPoint,
            Rectangle restricted,
            float cellSize,
            int minCellCount,
            Vector4 availableCoordinatesRange
        )
        {
            _spawnZoneDirection = spawnZoneDirection;
            _centralPoint = centralPoint;
            _cellSize = cellSize;
            _availableCoordinatesRange = availableCoordinatesRange;
            _lockedZone = CalculateLockedZone(restricted, cellSize);
            CalculateMainZone(minCellCount, cellSize);
            IsLocked = _mainZone.InCoordinatesRange(_availableCoordinatesRange) == false;
        }

        public bool IsLocked { get; private set; }

        public void RecalculateCells(int cellCount)
        {
            CalculateMainZone(cellCount, _cellSize);
            IsLocked = _mainZone.InCoordinatesRange(_availableCoordinatesRange) == false;
        }

        public void CalculateMainZone(int minCellCount, float cellRectSize = 1f, int maxIterationsCount = 1000)
        {
            float minWidth = _lockedZone.width;
            float minHeight = _lockedZone.height;
            float restrictArea = _lockedZone.width * _lockedZone.height;

            float minFreeAreaNeeded = minCellCount * cellRectSize * cellRectSize;
            int iterationsCount = 0;

            while (minWidth * minHeight - restrictArea < minFreeAreaNeeded)
            {
                if (++iterationsCount >= maxIterationsCount)
                    throw new Exception($"Reached iterations limit {maxIterationsCount}");
                minWidth += cellRectSize;
                minHeight += cellRectSize;
            }

            _width = minWidth;
            _height = minHeight;
            _topLeftCornerCoordinates = CalculateTLCorner();
            _mainZone = new Rectangle(_topLeftCornerCoordinates.x, _topLeftCornerCoordinates.y,
                minWidth,
                minHeight);
        }

        private Rectangle CalculateLockedZone(Rectangle restrictRectangle, float cellSize)
        {
            Vector2 TL;
            float halfWidth = restrictRectangle.width / 2;
            float halfHeight = restrictRectangle.height / 2;

            float deltaCellWidth = halfWidth % cellSize;
            float deltaCellHeight = halfHeight % cellSize;

            float width = deltaCellWidth == 0 ? halfWidth : halfWidth + (cellSize - deltaCellWidth);
            float height = deltaCellHeight == 0 ? halfHeight : halfHeight + (cellSize - deltaCellHeight);

            switch (_spawnZoneDirection)
            {
                case SpawnZoneDirection.I:
                    TL = new Vector2(_centralPoint.x, _centralPoint.y + height);
                    break;
                case SpawnZoneDirection.II:
                    TL = new Vector2(_centralPoint.x, _centralPoint.y);
                    break;
                case SpawnZoneDirection.III:
                    TL = new Vector2(_centralPoint.x - width, _centralPoint.y);
                    break;
                case SpawnZoneDirection.IV:
                    TL = new Vector2(_centralPoint.x - width, _centralPoint.y + height);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Rectangle(TL.x, TL.y, width, height);
        }

        private Vector2 CalculateTLCorner()
        {
            switch (_spawnZoneDirection)
            {
                case SpawnZoneDirection.I:
                    return new Vector2(_centralPoint.x, _centralPoint.y + _height);
                case SpawnZoneDirection.II:
                    return _centralPoint;
                case SpawnZoneDirection.III:
                    return new Vector2(_centralPoint.x - _width, _centralPoint.y);
                case SpawnZoneDirection.IV:
                    return new Vector2(_centralPoint.x - _width, _centralPoint.y + _height);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public Rectangle GetMainRectangle() => _mainZone;

        public Rectangle GetRestrictRectangle() => _lockedZone;

        public List<Vector2> GetAvailableGridPoints()
        {
            List<Vector2> result = new List<Vector2>();
            foreach (Rectangle allowedRect in Rectangle.difference(_mainZone, _lockedZone))
                result.AddRange(RectGrid.CalculatePoints(_cellSize, allowedRect));
            return result;
        }
    }
}