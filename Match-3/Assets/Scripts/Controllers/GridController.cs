using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Level;
using ModestTree;
using Pools;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D;
using UtilitiesAndHelpers;
using View;
using Zenject;
using Random = System.Random;


namespace Controllers
{
    [Serializable]
    public struct CellIndexInGrid : IEquatable<CellIndexInGrid>, IComparable<CellIndexInGrid>
    {
        public int RowIndex;
        public int ColumnIndex;

        private static bool IsEquals(CellIndexInGrid first, CellIndexInGrid second)
        {
            return first.ColumnIndex == second.ColumnIndex && first.RowIndex == second.RowIndex;
        }

        public bool Equals(CellIndexInGrid other)
        {
            return IsEquals(this, other);
        }

        public override bool Equals(object obj)
        {
            return obj is CellIndexInGrid other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(RowIndex, ColumnIndex);
        }

        public static bool operator ==(CellIndexInGrid first, CellIndexInGrid second)
        {
            return IsEquals(first, second);
        }

        public static bool operator !=(CellIndexInGrid first, CellIndexInGrid second)
        {
            return !IsEquals(first, second);
        }

        public static bool operator ==(CellIndexInGrid first, (int rowIndex, int columnIndex) second)
        {
            return IsEquals(first,
                new CellIndexInGrid { RowIndex = second.rowIndex, ColumnIndex = second.columnIndex });
        }

        public static bool operator !=(CellIndexInGrid first, (int rowIndex, int columnIndex) second)
        {
            return IsEquals(first,
                new CellIndexInGrid { RowIndex = second.rowIndex, ColumnIndex = second.columnIndex });
        }

        public int CompareTo(CellIndexInGrid other)
        {
            return (RowIndex, ColumnIndex).CompareTo((other.RowIndex, other.ColumnIndex));
        }
    }

    public interface IInitGrid
    {
        void GenerateGrid(CellType[,] cells);
    }

    public interface ISelectCell
    {
        void SelectCell(CellIndexInGrid id);
    }

    public interface IHighlightable
    {
        public void Highlight(bool status);
    }


    public interface ICellIdentifier
    {
        CellIndexInGrid Id { get; }
    }

    public interface IMatch
    {
        void Match();
    }

    public interface IWorldCellPosition
    {
        Vector3? CellPosition { get; }
    }

    public interface ICell : ICellIdentifier, IWorldCellPosition,
        IHighlightable, IMatch
    {
        CellType CellType { get; }

        public void SetData(CellIndexInGrid indexInGrid, CellType cellType,
            Vector3 position, Sprite sprite, Action<ICell> onMatch);

        public Task Shift(CellIndexInGrid indexInGrid, Vector3 position, Func<ICell, Task> onFinish,
            bool onMatchShift = false);
    }

    public class Cell : ICell
    {
        public CellIndexInGrid Id { get; private set; }
        public CellType CellType { get; private set; }
        public Vector3? CellPosition => _cellView?.CellPosition;

        private ICellView _cellView;

        private Action<ICell> _onMatch;

        public Cell(ICellView cellView)
        {
            _cellView = cellView;
        }

        public void SetData(CellIndexInGrid id, CellType cellType, Vector3 position, Sprite sprite,
            Action<ICell> onMatch)
        {
            Id = id;
            CellType = cellType;
            _onMatch = onMatch;

            _cellView.SetData(CellType is CellType.Empty
                ? null
                : sprite, Id, position);
        }

        public async Task Shift(CellIndexInGrid id, Vector3 position, Func<ICell, Task> onFinish,
            bool onMatchShift = false)
        {
            Id = id;
            await _cellView.Shift(id, position, onMatchShift);
            await onFinish.Invoke(this);
        }

        public void Highlight(bool status)
        {
            _cellView.Highlight(status);
        }

        public void Match()
        {
            _cellView.Match();
            _onMatch?.Invoke(this);
        }
    }

    public interface IGrid
    {
        void SetData(float cellSize);
        void RegisterCell(CellIndexInGrid id, CellType cellType, Vector3 position);
        Task<bool> SelectCell(CellIndexInGrid id);
    }

    public class GridInstance : IGrid
    {
        private IProcessMatch _processMatch;
        private ICell[,] _cells;
        private Vector3[,] _cellPositions;
        private ICell _selectedCell;
        private IPool<ICell> _cellPool;
        private SpriteAtlas _cellSpriteAtlas;
        private float _spawnYPosition;
        private List<CellType> _cellTypes;

        public GridInstance((int rowCount, int columnCount) gridData, IPool<ICell> cellPool,
            SpriteAtlas cellSpriteAtlas, IProcessMatch processMatch)
        {
            var rowCount = gridData.rowCount;
            var columnCount = gridData.columnCount;
            _cells = new ICell[rowCount, columnCount];
            _cellPositions = new Vector3[rowCount, columnCount];
            _cellSpriteAtlas = cellSpriteAtlas;
            _cellPool = cellPool;
            _processMatch = processMatch;

            _cellTypes =
                (List<CellType>)Enum.GetValues(typeof(CellType)).ConvertTo(typeof(List<CellType>));
            _cellTypes.Remove(CellType.Empty);
        }

        private void ProcessMatchedCell(ICell cell)
        {
            var cellId = cell.Id;
            _cells[cellId.RowIndex, cellId.ColumnIndex] = null;
            _cellPool.Return(cell);
            _processMatch.ProcessMatch(cell.CellType);
        }

        public void SetData(float cellSize)
        {
            var cellPosition = _cellPositions[0, 0];
            _spawnYPosition = cellPosition.y + cellSize;
        }

        public void RegisterCell(CellIndexInGrid id, CellType cellType, Vector3 position)
        {
            var cell = cellType is CellType.Empty ? null : SpawnCell(id, cellType, position);

            _cells[id.RowIndex, id.ColumnIndex] = cell;
            _cellPositions[id.RowIndex, id.ColumnIndex] = position;
        }

        private ICell SpawnRandomCell(CellIndexInGrid id)
        {
            var targetPosition = _cellPositions[id.RowIndex, id.ColumnIndex];
            var spawnPosition = new Vector3(targetPosition.x, _spawnYPosition, targetPosition.z);
            return SpawnCell(id, GetRandomType(), spawnPosition);
        }


        private ICell SpawnCell(CellIndexInGrid id, CellType cellType, Vector3 position)
        {
            var cell = _cellPool.Get();
            cell.SetData(id, cellType, position,
                _cellSpriteAtlas.GetSprite(cellType is CellType.Empty ? null : Helpers.TypeToName[cellType]),
                ProcessMatchedCell);
            return cell;
        }

        private Random _random = new Random();

        private CellType GetRandomType()
        {
            return _cellTypes[_random.Next(0, _cellTypes.Count)];
        }

        public async Task<bool> SelectCell(CellIndexInGrid id)
        {
            if (_swapTask is { })
                return false;

            if (_selectedCell is null)
            {
                _selectedCell = _cells[id.RowIndex, id.ColumnIndex];
                _selectedCell.Highlight(true);
                return false;
            }

            var selectedCellId = _selectedCell.Id;

            if (selectedCellId == id)
                return false;


            var currentCell = _cells[id.RowIndex, id.ColumnIndex];
            var currentCellId = currentCell.Id;
            _swapTask = SwapTiles(_selectedCell, currentCell);
            await _swapTask;
            if (!_swapTask.Result)
            {
                _swapTask = null;
                return false;
            }

            FindMatches(new List<CellIndexInGrid> { selectedCellId, currentCellId });

            _selectedCell.Highlight(false);
            _selectedCell = null;

            return true;
        }

        private Task<bool> _swapTask;

        private async Task<bool> SwapTiles(ICell firstCell, ICell secondCell)
        {
            var firstCellId = firstCell.Id;
            var secondCellId = secondCell.Id;

            var difference = (Math.Abs(firstCellId.ColumnIndex - secondCellId.ColumnIndex),
                Math.Abs(firstCellId.RowIndex - secondCellId.RowIndex));

            if (difference.Item1 > 1 || difference.Item2 > 1)
                return false;

            var swapTasks = new List<Task>
            {
                firstCell.Shift(secondCellId,
                    _cellPositions[secondCellId.RowIndex, secondCellId.ColumnIndex], ProcessCellShift),
                secondCell.Shift(firstCellId,
                    _cellPositions[firstCellId.RowIndex, firstCellId.ColumnIndex], ProcessCellShift)
            };

            await Task.WhenAll(swapTasks);

            return true;
        }

        private Task ProcessCellShift(ICell cell)
        {
            var cellId = cell.Id;
            _cells[cellId.RowIndex, cellId.ColumnIndex] = cell;
            return Task.CompletedTask;
        }

        private async void FindMatches(IList<CellIndexInGrid> cells)
        {
            var matches = new List<CellIndexInGrid>();

            for (int i = 0; i < cells.Count; i++)
            {
                var currentCellId = cells[i];
                if (_cells[currentCellId.RowIndex, currentCellId.ColumnIndex] is null)
                    continue;

                var matched = await FindMatch(currentCellId);
                var matchedIds = matched.Select(cell => cell.Id).ToList();
                matches.AddRange(matchedIds);
                Match(matched);
            }

            if (matches.IsEmpty())
            {
                _swapTask = null;
                return;
            }

            ReFillCells(matches);
        }

        private async Task<IList<ICell>> FindMatch(CellIndexInGrid cellId)
        {
            var cell = _cells[cellId.RowIndex, cellId.ColumnIndex];
            var matches = new List<ICell>();
            var buffer = new List<ICell>();
            GetMatchingNeighbours(cell, Directions.Left, buffer);
            GetMatchingNeighbours(cell, Directions.Right, buffer);

            if (buffer.Count >= 2)
            {
                matches.AddRange(buffer);
            }

            buffer.Clear();

            GetMatchingNeighbours(cell, Directions.Up, buffer);
            GetMatchingNeighbours(cell, Directions.Down, buffer);

            if (buffer.Count >= 2)
            {
                matches.AddRange(buffer);
            }

            if (!matches.IsEmpty())
                matches.Add(cell);

            await Task.Yield();
            return matches;
        }

        private void GetMatchingNeighbours(ICell previousCell, Directions directions, IList<ICell> matches)
        {
            var previousCellId = previousCell.Id;
            var currentCellId = directions switch
            {
                Directions.Up => (previousCellId.RowIndex,
                    Math.Clamp(previousCellId.ColumnIndex - 1, 0, SharedData.CurrentColumnCount - 1)),
                Directions.Down => (previousCellId.RowIndex,
                    Math.Clamp(previousCellId.ColumnIndex + 1, 0, SharedData.CurrentColumnCount - 1)),
                Directions.Left => (Mathf.Clamp(previousCellId.RowIndex - 1, 0, SharedData.CurrentRowCount - 1),
                    previousCellId.ColumnIndex),
                Directions.Right => (Mathf.Clamp(previousCellId.RowIndex + 1, 0, SharedData.CurrentRowCount - 1),
                    previousCellId.ColumnIndex),
            };

            if (previousCellId == currentCellId)
                return;

            var currentCell = _cells[currentCellId.Item1, currentCellId.Item2];

            if (currentCell is null ||
                currentCell.CellType != previousCell.CellType)
                return;

            matches.Add(currentCell);
            GetMatchingNeighbours(currentCell, directions, matches);
        }

        private void Match(IList<ICell> cells)
        {
            for (int i = 0; i < cells.Count; i++)
            {
                cells[i].Match();
            }
        }

        private async void ReFillCells(IList<CellIndexInGrid> matchedIds)
        {
            var sortedCells = matchedIds.OrderBy(data => data).ToList();
            var shiftTasks = new List<Task>();
            matchedIds.Clear();

            while (!sortedCells.IsEmpty())
            {
                var currentMatchedCellId = sortedCells[0];
                var currentMatchedCellPosition =
                    _cellPositions[currentMatchedCellId.RowIndex, currentMatchedCellId.ColumnIndex];
                var upwardsCell = currentMatchedCellId.ColumnIndex == 0
                    ? null
                    : _cells[currentMatchedCellId.RowIndex, currentMatchedCellId.ColumnIndex - 1];
                var canShift = upwardsCell is { };

                ICell shiftCell;

                if (canShift)
                {
                    sortedCells[0] = upwardsCell.Id;
                    shiftCell = upwardsCell;
                }
                else
                {
                    var cell = SpawnRandomCell(currentMatchedCellId);
                    shiftCell = cell;
                    sortedCells.RemoveAt(0);
                }

                matchedIds.Add(currentMatchedCellId);
                shiftTasks.Add(
                    shiftCell.Shift(currentMatchedCellId, currentMatchedCellPosition, ProcessCellShift, true));
            }

            await Task.WhenAll(shiftTasks);
            FindMatches(matchedIds);
        }
    }

    public class GridController : ControllerBase, IInitGrid, ISelectCell
    {
        private IGrid _gridInstance;
        private IProcessMove _processMove;
        private IProcessMatch _processMatch;
        private MainCamera _mainCamera;
        private SpriteAtlas _cellSpriteAtlas;
        private CellView _cellPrefab;
        private Vector3 _playingAreaBounds;
        private Vector3 _screenBounds;

        [Inject]
        void Construct(MainCamera mainCamera, IGetCellAtlas getCellAtlas,
            IGetCell getCell, IProcessMove processMove, IProcessMatch processMatch
            , IGetLevelSettings getLevelSettings)
        {
            _mainCamera = mainCamera;
            _cellSpriteAtlas = getCellAtlas.GetCellAtlas();
            _cellPrefab = getCell.GetCellPrefab();
            _processMove = processMove;
            _processMatch = processMatch;
        }

        public override Task Initialize()
        {
            _screenBounds = _mainCamera.Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
                _mainCamera.Camera.transform.position.z));
            _playingAreaBounds =
                new Vector3(Math.Abs(_screenBounds.x * 2), Math.Abs(_screenBounds.y * 2), _screenBounds.z);
            return base.Initialize();
        }

        public void GenerateGrid(CellType[,] cells)
        {
            if (cells is null)
                return;

            var cellPool = new CellPool<ICell>(new CellFactory(_cellPrefab));
            var rowCount = cells.GetRowCount();
            var columnCount = cells.GetColumnCount();
            SharedData.CurrentColumnCount = columnCount;
            SharedData.CurrentRowCount = rowCount;
            _gridInstance = new GridInstance((rowCount, columnCount), cellPool,
                _cellSpriteAtlas, _processMatch);

            var areaHeight = _playingAreaBounds.y * (1 - SharedData.HeightOffset * 2);
            var areaWidth = _playingAreaBounds.x * (1 - SharedData.WidthOffset * 2);

            var approximateGridDimensions = Math.Min(areaHeight, areaWidth);
            var approximateCellDimensions =
                approximateGridDimensions / Math.Max(SharedData.MaxRowSize, SharedData.MaxColumnSize);

            var halfCell = approximateCellDimensions / 2f;
            var isYEven = columnCount % 2 is 0;
            var isXEven = rowCount % 2 is 0;
            var middleYCell = (int)columnCount / 2;
            var middleXCell = (int)rowCount / 2;

            for (int i = 0; i < rowCount; i++)
            {
                var currentXPos = -((isXEven ? halfCell : 0) +
                                    (approximateCellDimensions * ((middleXCell - (isXEven ? 1 : 0)) - i)));

                for (int j = 0; j < columnCount; j++)
                {
                    var currentYPos = (isYEven ? halfCell : 0) +
                                      (approximateCellDimensions * ((middleYCell - (isYEven ? 1 : 0)) - j));
                    var currentCellPos = new Vector3(currentXPos, currentYPos, 0f);
                    var currentCellType = cells[i, j];

                    _gridInstance.RegisterCell(new CellIndexInGrid { RowIndex = i, ColumnIndex = j },
                        currentCellType, currentCellPos);
                }
            }

            _gridInstance.SetData(approximateCellDimensions);
        }

        private Task<bool> _selectTask;

        public async void SelectCell(CellIndexInGrid id)
        {
            if (_selectTask is { })
                return;

            _selectTask = _gridInstance.SelectCell(id);
            await _selectTask;
            if (_selectTask.Result)
                _processMove.ProcessMove();

            _selectTask = null;
        }
    }
}