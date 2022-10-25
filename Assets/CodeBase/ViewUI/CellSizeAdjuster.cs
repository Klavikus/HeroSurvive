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
            int cellInRow = _gridLayoutGroup.constraintCount;
            float paddingOffset = _gridLayoutGroup.padding.left + _gridLayoutGroup.padding.right;
            float spacingOffset = _gridLayoutGroup.spacing.x * (cellInRow - 1);
            float cellSize = (_rectTransform.rect.width - paddingOffset - spacingOffset) / cellInRow;
            return new Vector2(cellSize, cellSize);
        }
    }
}