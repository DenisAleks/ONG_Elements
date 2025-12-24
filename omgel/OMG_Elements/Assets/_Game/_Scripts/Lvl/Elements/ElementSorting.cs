using UnityEngine;

namespace _Game._Scripts.Lvl.Elements
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ElementSorting : MonoBehaviour
    {
        private const int DiagonalStride = 100;
        private const int RowStride = 10;

        [SerializeField] private SpriteRenderer _renderer;
        
        //NOT WORK for Vertical at all. TODO think better
        public void UpdateSortingOrder(Cell cell)
        {
            var diagonal = cell.MatrixPosition.x + cell.MatrixPosition.y;

            _renderer.sortingOrder = diagonal * DiagonalStride
                                     + cell.MatrixPosition.y * RowStride
                                     + cell.MatrixPosition.x;
        }
    }
}