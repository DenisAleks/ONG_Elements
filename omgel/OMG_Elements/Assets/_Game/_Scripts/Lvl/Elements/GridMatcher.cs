using System;
using System.Collections.Generic;
using _Game._Scripts.Model.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Game._Scripts.Lvl.Elements
{
    /// <summary>
    ///     Encapsulates the whole “find lines-of-3+ and build connected areas” logic.
    /// </summary>
    public class GridMatcher
    {
        private readonly GridController _grid;

        public GridMatcher(GridController grid)
        {
            _grid = grid;
        }

        public List<List<Cell>> FindMatches()
        {
            var cellGrid = _grid.GetCellGrid();
            var rows = cellGrid.GetLength(0);
            var cols = cellGrid.GetLength(1);

            var tripleCells = new HashSet<Cell>();

            // horizontal scan
            for (var y = 0; y < rows; y++)
            {
                var x = 0;
                while (x < cols)
                {
                    var c = cellGrid[y, x];
                    if (!c || c.ElementType == ElementType.None)
                    {
                        x++;
                        continue;
                    }

                    var type = c.ElementType;
                    var len = 1;
                    while (x + len < cols &&
                           cellGrid[y, x + len] &&
                           cellGrid[y, x + len].ElementType == type)
                    {
                        len++;
                    }

                    if (len >= 3)
                    {
                        for (var i = 0; i < len; i++)
                        {
                            tripleCells.Add(cellGrid[y, x + i]);
                        }
                    }

                    x += len;
                }
            }

            // vertical scan
            for (var x = 0; x < cols; x++)
            {
                var y = 0;
                while (y < rows)
                {
                    var c = cellGrid[y, x];
                    if (!c || c.ElementType == ElementType.None)
                    {
                        y++;
                        continue;
                    }

                    var type = c.ElementType;
                    var len = 1;
                    while (y + len < rows &&
                           cellGrid[y + len, x] &&
                           cellGrid[y + len, x].ElementType == type)
                    {
                        len++;
                    }

                    if (len >= 3)
                    {
                        for (var i = 0; i < len; i++)
                        {
                            tripleCells.Add(cellGrid[y + i, x]);
                        }
                    }

                    y += len;
                }
            }

            if (tripleCells.Count == 0)
            {
                return null;
            }

            var groups = new List<List<Cell>>();
            var visited = new HashSet<Cell>();
            
            foreach (var start in tripleCells)
            {
                if (visited.Contains(start))
                {
                    continue;
                }

                var group = new List<Cell>();
                var stack = new Stack<Cell>();
                stack.Push(start);

                while (stack.Count > 0)
                {
                    var cur = stack.Pop();
                    if (visited.Contains(cur))
                    {
                        continue;
                    }

                    visited.Add(cur);
                    group.Add(cur);

                    foreach (SwipeDirection dir in Enum.GetValues(typeof(SwipeDirection)))
                    {
                        if (!_grid.TryGetNeighbor(cur, dir, out var n) ||
                            n.ElementType != start.ElementType ||
                            visited.Contains(n))
                        {
                            continue;
                        }

                        stack.Push(n);
                    }
                }

                groups.Add(group);
            }

            return groups;
        }
    }
}