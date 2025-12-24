using _Game._Scripts.Lvl.Elements;
using _Game._Scripts.Model.Enums;
using _Game._Scripts.Settings;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace _Game._Scripts.Lvl.Interaction
{
    /// <summary>
    /// Reads player input
    /// </summary>
    public sealed class GridInputReader : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
       
        private IGridInteractionService _interaction;
        private bool _pressed;
        private Cell _startCell;

        private Vector2 _startScreenPos;
        private GameSettings _gameSettings;
        
        [Inject]
        public void Construct(GameSettings settings, IGridInteractionService interaction)
        {
            _interaction = interaction;
            _gameSettings = settings;
        }


        private void Update()
        {
            if (Pointer.current == null)
            {
                return;
            }

            if (Pointer.current.press.wasPressedThisFrame)
            {
                Begin(Pointer.current.position.ReadValue());
            }
            else if (Pointer.current.press.wasReleasedThisFrame)
            {
                End(Pointer.current.position.ReadValue());
            }
        }
        
        private void Begin(Vector2 screenPos)
        {
            _startCell = RaycastCell(screenPos);
            if (_startCell == null)
            {
                return;
            }

            if (!_interaction.CanInteract(_startCell))
            {
                _startCell = null;
                return;
            }

            _startScreenPos = screenPos;
            _pressed = true;
        }

        private void End(Vector2 screenPos)
        {
            if (!_pressed || _startCell == null)
            {
                return;
            }

            var delta = screenPos - _startScreenPos;
            if (delta.magnitude < _gameSettings.GridSettings.MinSwipeDistance)
            {
                Reset();
                return;
            }

            var direction = ResolveDirection(delta);
            _interaction.TrySwipe(_startCell, direction);

            Reset();
        }

        private Cell RaycastCell(Vector2 screenPos)
        {
            var world = _camera.ScreenToWorldPoint(screenPos);
            var hit = Physics2D.Raycast(world, Vector2.zero);
            return hit.collider ? hit.collider.GetComponent<Cell>() : null;
        }

        private SwipeDirection ResolveDirection(Vector2 delta)
        {
            return Mathf.Abs(delta.x) > Mathf.Abs(delta.y)
                ? delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left
                : delta.y > 0
                    ? SwipeDirection.Up
                    : SwipeDirection.Down;
        }
        
        private void Reset()
        {
            _pressed = false;
            _startCell = null;
        }
    }
}