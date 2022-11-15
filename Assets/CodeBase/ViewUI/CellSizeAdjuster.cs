using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.ViewUI
{
    [RequireComponent(typeof(RectTransform))]
    public class CellSizeAdjuster : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;
        [SerializeField] private RectTransform _rectTransform;

        private void OnRectTransformDimensionsChange() => _gridLayoutGroup.cellSize = CalculateNewSize();

        private Vector2 CalculateNewSize()
        {
            int cellsInRow = _gridLayoutGroup.constraintCount;
            float paddingOffset = _gridLayoutGroup.padding.left + _gridLayoutGroup.padding.right;
            float spacingOffset = _gridLayoutGroup.spacing.x * (cellsInRow - 1);
            float cellSize = (_rectTransform.rect.width - paddingOffset - spacingOffset) / cellsInRow;
            return new Vector2(cellSize, cellSize);
        }
    }
}