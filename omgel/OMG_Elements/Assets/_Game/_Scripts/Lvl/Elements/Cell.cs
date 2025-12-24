using _Game._Scripts.Model.Enums;
using UnityEngine;

namespace _Game._Scripts.Lvl.Elements
{
    //Stores logical data of the single grid cell. No any logic here
    public class Cell : MonoBehaviour
    {
        public Vector2Int MatrixPosition { get; private set; }
        public ElementType ElementType { get; private set; }
        public CellState State { get; private set; }
      
        public bool IsInteractable => State == CellState.Idle;

        [SerializeField] private SpriteRenderer _spriteRenderer;
        
        public void Init(Vector2Int position, float scale)
        {
            MatrixPosition = position;
            transform.localScale = Vector3.one * scale;
            gameObject.name = $"Cell[{position.x}.{position.y}]";
        }

        public void SetElement(ElementType type) => ElementType = type;

        public void OnDespawned() => ElementType = ElementType.None;
        
        public void SetState(CellState state) => State = state;

        public override string ToString()
        {
            return $"Cell[{MatrixPosition.x}.{MatrixPosition.y}]";
        }
    }
}