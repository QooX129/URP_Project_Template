using UnityEngine;
using UnityEngine.UI;

namespace AppName_Rename
{
    public class FlexibleGridLayout : LayoutGroup
    {
        private enum FitType
        {
            UNIFORM,
            WIDTH,
            HEIGHT,
            FIXEDROWS,
            FIXEDCOLUMNS
        }

        [SerializeField] private FitType fitType = FitType.UNIFORM;
        [SerializeField] private int rows;
        [SerializeField] private int columns;
        [SerializeField] private Vector2 cellSize;
        [SerializeField] private Vector2 spacing;
        [SerializeField] private bool fitX;
        [SerializeField] private bool fitY;

        public override void CalculateLayoutInputHorizontal()
        {
            base.CalculateLayoutInputHorizontal();

            if (fitType is FitType.WIDTH or FitType.HEIGHT or FitType.UNIFORM)
            {
                var sqrt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrt);
                switch (fitType)
                {
                    case FitType.WIDTH:
                        fitX = true;
                        fitY = false;
                        break;
                    case FitType.HEIGHT:
                        fitX = false;
                        fitY = true;
                        break;
                    case FitType.UNIFORM:
                        fitX = true;
                        fitY = true;
                        break;
                }
            }

            switch (fitType)
            {
                case FitType.WIDTH or FitType.FIXEDCOLUMNS:
                    rows = Mathf.CeilToInt(transform.childCount / (float)columns);
                    break;
                case FitType.HEIGHT or FitType.FIXEDROWS:
                    columns = Mathf.CeilToInt(transform.childCount / (float)rows);
                    break;
            }

            var parentWidth = rectTransform.rect.width;
            var parentHeight = rectTransform.rect.height;

            var cellWidth = parentWidth / columns - ((spacing.x / columns) * (columns - 1)) -
                            padding.left / (float)columns -
                            padding.right / (float)columns;

            var cellHeight = parentHeight / rows - (spacing.y / rows * (rows - 1)) -
                             padding.top / (float)rows -
                             padding.bottom / (float)rows;

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            for (int i = 0; i < rectChildren.Count; i++)
            {
                var rowCount = i / columns;
                var columnCount = i % columns;

                var item = rectChildren[i];

                var xPos = cellSize.x * columnCount + spacing.x * columnCount + padding.left;
                var yPos = cellSize.y * rowCount + spacing.y * rowCount + padding.right;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);
            }
        }

        public override void CalculateLayoutInputVertical()
        {
        }

        public override void SetLayoutHorizontal()
        {
        }

        public override void SetLayoutVertical()
        {
        }
    }
}