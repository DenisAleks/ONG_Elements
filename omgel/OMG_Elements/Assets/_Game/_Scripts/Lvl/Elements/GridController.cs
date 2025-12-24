using System;
using System.Collections.Generic;
using _Game._Scripts.Model.Data.Save;
using _Game._Scripts.Model.Enums;
using _Game._Scripts.Model.Levels;
using _Game._Scripts.Settings;
using _Game._Scripts.Utils;
using UnityEngine;
using VContainer.Unity;

namespace _Game._Scripts.Lvl.Elements
{
    /// <summary>
    ///     Creates the logical grid, keeps cell-element mapping
    ///     and answers low-level queries.  All *actions* are delegated
    ///     to specialised helpers.
    /// </summary>
    public class GridController : ITickable, IDisposable
    {
        private const int IdleStateCheckInterval = 5;

        public int Rows => _cells.GetLength(0);
        public int Columns => _cells.GetLength(1);
        public int LogicalColumns => _level.Columns + ExtraSideCells * 2;
        public bool IsBusy { get; private set; }
        private int XOffset => ExtraSideCells;

        private GridPositioner Layout { get; set; }

        private const int ExtraSideCells = 1;

        private readonly CellPool _cellPool;
        private readonly ElementPool _elementPool;
        private readonly GameEventBus _eventBus;
        private readonly GameSettings _gameSettings;
        private GridBlaster _blaster;

        private Cell[,] _cells;
        private Dictionary<Cell, Element> _cellToElement;
        private List<Element> _elements;
        private GridGravity _gravity;

        private LevelTable _level;

        private GridMatcher _matcher;
        private GridSwapper _swapper;
        private int _tickCounter = 0;
        private bool _isPassed;


        public GridController(CellPool cellPool, ElementPool elementPool,
            GameSettings gameSettings, GameEventBus eventBus)
        {
            _cellPool = cellPool;
            _elementPool = elementPool;
            _gameSettings = gameSettings;
            _eventBus = eventBus;
        }

        public void Dispose()
        {
            if (_cells == null)
            {
                return;
            }

            foreach (var c in _cells)
            {
                if (c != null)
                {
                    _cellPool.Release(c);
                }
            }

            foreach (var e in _elements)
            {
                if (e != null)
                {
                    _elementPool.Release(e);
                }
            }

            _cells = null;
            _elements = null;
            Layout = null;
            _matcher = null;
            _swapper = null;
            _blaster = null;
            _isPassed = false;
        }
        
        public void Init(LevelTable level)
        {
            _level = level;
            var scale = _gameSettings.GridSettings.GetElementScale(level.Columns);

            CreateLayout(level);
            CreateCells(level, scale);
            CreateElements(level, scale);

            _matcher = new GridMatcher(this);
            _swapper = new GridSwapper(this, _gameSettings);
            _blaster = new GridBlaster(this, _elementPool);
            _gravity = new GridGravity(this, _gameSettings);
        }

        public void RestoreFromSave(LevelTable level, GridSaveData data)
        {
            _level = level;

            var scale = _gameSettings.GridSettings.GetElementScale(level.Columns);

            CreateLayout(level);
            CreateCells(level, scale);

            _elements = new List<Element>();
            _cellToElement = new Dictionary<Cell, Element>();

            for (var y = 0; y < data.Rows; y++)
            {
                for (var x = 0; x < data.Columns; x++)
                {
                    var cellX = x + XOffset;
                    var cell = _cells[y, cellX];

                    var type = data.Cells[y * data.Columns + x];
                    cell.SetElement(type);

                    if (type == ElementType.None)
                    {
                        continue; // no element to create
                    }

                    var element = _elementPool.Get(type);
                    element.Init(_gameSettings, scale);

                    BindElement(element, cell);
                    element.transform.localPosition = Layout.GetLocalPosition(cellX, y);
                    _elements.Add(element);
                }
            }

            _matcher = new GridMatcher(this);
            _swapper = new GridSwapper(this, _gameSettings);
            _blaster = new GridBlaster(this, _elementPool);
            _gravity = new GridGravity(this, _gameSettings);
        }

        public void Tick()
        {
            if (_cells == null)
            {
                return;
            }
            
            _tickCounter++;
            if (_tickCounter < IdleStateCheckInterval)
            {
                return;
            }
            _tickCounter = 0;

            TrySetIdle();
        }
        
        public void RemoveElement(Element element)
        {
            _elements.Remove(element);
            foreach (var kv in _cellToElement)
            {
                if (kv.Value == element)
                {
                    _cellToElement.Remove(kv.Key);
                    break;
                }
            }
        }

        public void SetBusy() => IsBusy = true;

        public void SetIdle()
        {
            if (IsBusy)
            {
                IsBusy = false;
                _eventBus.RaiseGridBecomeIdle();
            }
        }

        public bool TryGetNeighbor(Cell cell, SwipeDirection dir, out Cell neighbor)
            => TryGetCell(cell.MatrixPosition + dir.ToOffset(), out neighbor);

        public bool TryGetElement(Cell cell, out Element element)
            => _cellToElement.TryGetValue(cell, out element);

        public void Normalize() => FindMatches();

        public void ApplyGravity() => _gravity.ApplyGravity();

        public void MoveCell(Cell from, Cell to) => _swapper.Swap(from, to);

        public Cell[,] GetCellGrid() => _cells;

        public void BindElement(Element e, Cell c)
        {
            e.UpdateSortingOrder(c);
            _cellToElement[c] = e;
        }

        public Element ExtractElement(Cell c)
        {
            if (_cellToElement.TryGetValue(c, out var e))
            {
                _cellToElement.Remove(c);
                return e;
            }

            return null;
        }

        public GridSaveData CreateSaveData()
        {
            var rows = _cells.GetLength(0);
            var columns = _level.Columns;

            var hasAnyElement = false;
            var cellsArray = new ElementType[rows * columns];

            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < columns; x++)
                {
                    var cellX = x + XOffset;
                    var cell = _cells[y, cellX];
                    var type = cell.ElementType;
                    cellsArray[y * columns + x] = type;

                    if (type != ElementType.None)
                    {
                        hasAnyElement = true;
                    }
                }
            }

            if (!hasAnyElement)
            {
                Debug.Log($"No elements to save - level is passed");
                return null; // nothing to save
            }

            return new GridSaveData
            {
                Rows = rows,
                Columns = columns,
                Cells = cellsArray
            };
        }

        private void TrySetIdle()
        {
            if (!IsBusy || _isPassed)
            {
                return;
            }
            
            var anyBusy = false;
            var hasElements = false;

            foreach (var cell in _cells)
            {
                if (cell == null)
                {
                    continue;
                }

                if (cell.ElementType != ElementType.None)
                {
                    hasElements = true;
                }

                if (cell.State != CellState.Idle)
                {
                    anyBusy = true;
                }
            }

            if (!hasElements)
            {
                _isPassed = true;
                _eventBus.RaiseLevelPassed();
                return;
            }

            var canSave = !anyBusy;
            
            // Debug.Log($"Tick {Time.frameCount} hasElements-{hasElements}, anyBusy-{anyBusy}, canSave-{canSave}");
            if (canSave)
            {
                SetIdle();
            }
        }

        private void FindMatches()
        {
            var groups = _matcher.FindMatches();
            if (groups == null || groups.Count == 0)
            {
                return;
            }

            _blaster.BlastGroups(groups);
        }

        private bool TryGetCell(Vector2Int pos, out Cell cell)
        {
            if (pos.x < 0 || pos.x >= Columns || pos.y < 0 || pos.y >= Rows)
            {
                cell = null;
                return false;
            }

            cell = _cells[pos.y, pos.x];
            return cell != null;
        }

        private void CreateLayout(LevelTable lvl)
        {
            var gs = _gameSettings.GridSettings;
            var scale = gs.GetElementScale(lvl.Columns);
            var cellSize = gs.CellSizeDefault * scale;
            Layout = new GridPositioner(LogicalColumns, cellSize, gs.GridPositionY);
        }

        private void CreateCells(LevelTable lvl, float scale)
        {
            _cells = new Cell[lvl.Rows, LogicalColumns];
            for (var y = 0; y < lvl.Rows; y++)
            for (var x = 0; x < LogicalColumns; x++)
            {
                var cell = _cellPool.Get();
                _cells[y, x] = cell;
                cell.Init(new Vector2Int(x, y), scale);

                if (x >= XOffset && x < XOffset + lvl.Columns)
                {
                    cell.SetElement(lvl.Grid[y, x - XOffset]);
                }
                else
                {
                    cell.SetElement(ElementType.None);
                }

                cell.transform.localPosition = Layout.GetLocalPosition(x, y);
            }
        }

        private void CreateElements(LevelTable lvl, float scale)
        {
            _elements = new List<Element>();
            _cellToElement = new Dictionary<Cell, Element>();

            for (var y = 0; y < lvl.Rows; y++)
            for (var x = 0; x < lvl.Columns; x++)
            {
                var type = lvl.Grid[y, x];
                if (type == ElementType.None)
                {
                    continue;
                }

                var element = _elementPool.Get(type);
                element.Init(_gameSettings, scale);

                var cellX = x + XOffset;
                var cell = _cells[y, cellX];

                BindElement(element, cell);
                element.transform.localPosition = Layout.GetLocalPosition(cellX, y);
                _elements.Add(element);
            }
        }
    }
}