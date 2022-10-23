using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Level;
using ModestTree;
using Pools;
using Unity.VisualScripting;
using UnityEditor;
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

    public interface IGridBehaviour : IGridActive
    {
        void GenerateGrid(CellType[,] cells);
    }

    public interface IGridActive
    {
        bool IsActive { get; }
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

    public interface IMoveCell
    {
        Task Move(Vector3 position);
    }

    public interface ICell : ICellIdentifier, IWorldCellPosition,
        IHighlightable, IMatch, IMoveCell
    {
        CellType CellType { get; }

        public ICellView CellView { get; }

        public void SetData(CellIndexInGrid indexInGrid, CellType cellType,
            Vector3 position, Sprite sprite, Action<ICell> onMatch);

        public Task Shift(CellIndexInGrid indexInGrid, Vector3 position, Func<ICell, Task> onFinish,
            bool onMatchShift = false);
    }

    public class Cell : ICell
    {
        public CellIndexInGrid Id { get; private set; }
        public CellType CellType { get; private set; }
        public ICellView CellView => _cellView;
        public Vector3? CellPosition => _cellView?.CellPosition;

        private ICellView _cellView;
        private Action<ICell> _onMatch;

        public Cell(ICellView cellView)
        {
            _cellView = cellView;
        }

        public void SetData(CellIndexInGrid id, CellType cellType, Vector3 position,
            Sprite sprite, Action<ICell> onMatch)
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

        public async Task Move(Vector3 position)
        {
            await _cellView.Move(position);
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

    public interface IGrid : IGridActive
    {
        void RegisterCell(CellIndexInGrid id, CellType cellType, Vector3 position);
        Task<bool> SelectCell(CellIndexInGrid id);
    }

    public class CellComparer : IComparer<CellIndexInGrid>
    {
        public int Compare(CellIndexInGrid x, CellIndexInGrid y)
        {
            var columnCompareResult = -x.ColumnIndex.CompareTo(y.ColumnIndex);
            if (columnCompareResult != 0)
                return columnCompareResult;

            return x.RowIndex.CompareTo(y.RowIndex);
        }
    }

    public class GridInstance : IGrid
    {
        private Dictionary<int, Queue<CellIndexInGrid>> _rowToSpawnQueue = new();
        private Dictionary<int, Task> _rowToSpawnDelayTask = new();
        private Random _random = new();
        private IProcessMatch _processMatch;
        private ICell[,] _cells;
        private Vector3[,] _cellPositions;
        private ICell _selectedCell;
        private IPool<ICell> _cellPool;
        private SpriteAtlas _cellSpriteAtlas;
        private List<CellType> _cellTypes;
        private Task<bool> _swapTask;
        private float _spawnYPosition;
        private float _firstColumnPosition;

        public GridInstance((int rowCount, int columnCount) gridData, IPool<ICell> cellPool,
            SpriteAtlas cellSpriteAtlas, IProcessMatch processMatch, float spawnYPosition, float firstColumnPosition)
        {
            var rowCount = gridData.rowCount;
            var columnCount = gridData.columnCount;
            _cells = new ICell[rowCount, columnCount];
            _cellPositions = new Vector3[rowCount, columnCount];
            _cellSpriteAtlas = cellSpriteAtlas;
            _cellPool = cellPool;
            _processMatch = processMatch;
            _spawnYPosition = spawnYPosition;
            _firstColumnPosition = firstColumnPosition;

            _cellTypes =
                (List<CellType>)Enum.GetValues(typeof(CellType)).ConvertTo(typeof(List<CellType>));
            _cellTypes.Remove(CellType.Empty);

            for (int i = 0; i < rowCount; i++)
            {
                _rowToSpawnQueue[i] = new Queue<CellIndexInGrid>();
                _rowToSpawnDelayTask[i] = Task.CompletedTask;
            }
        }

        private void ProcessMatchedCell(ICell cell)
        {
            var cellId = cell.Id;
            _cells[cellId.RowIndex, cellId.ColumnIndex] = null;
            _cellPool.Return(cell);
            _processMatch.ProcessMatch(cell.CellType);
        }

        public void RegisterCell(CellIndexInGrid id, CellType cellType, Vector3 position)
        {
            var cell = cellType is CellType.Empty ? null : SpawnCell(id, cellType, position);

            _cells[id.RowIndex, id.ColumnIndex] = cell;
            _cellPositions[id.RowIndex, id.ColumnIndex] = position;
        }

        private async Task<ICell> SpawnRandomCell(CellIndexInGrid id)
        {
            while (_rowToSpawnQueue[id.RowIndex].Peek() != id)
            {
                await Task.Yield();
            }

            var targetPosition = _cellPositions[id.RowIndex, id.ColumnIndex];
            var spawnPosition = new Vector3(targetPosition.x, _spawnYPosition, targetPosition.z);
            var cell = SpawnCell(id, GetRandomType(), spawnPosition);
            return cell;
        }


        private ICell SpawnCell(CellIndexInGrid id, CellType cellType, Vector3 position)
        {
            var cell = _cellPool.Get();
            cell.SetData(id, cellType, position,
                _cellSpriteAtlas.GetSprite(cellType is CellType.Empty ? null : Helpers.TypeToName[cellType]),
                ProcessMatchedCell);
            return cell;
        }

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
            var sortedCells = new List<CellIndexInGrid>(matchedIds);
            var shiftTasks = new List<Task>();
            matchedIds.Clear();

            while (!sortedCells.IsEmpty())
            {
                sortedCells.Sort(new CellComparer());
                var currentCellsToShift =
                    sortedCells.Where(cell => cell.ColumnIndex == sortedCells[0].ColumnIndex).ToList();
                sortedCells.RemoveRange(0, currentCellsToShift.Count);

                for (int i = 0; i < currentCellsToShift.Count; i++)
                {
                    var currentMatchedCellId = currentCellsToShift[i];
                    var currentMatchedCellPosition =
                        _cellPositions[currentMatchedCellId.RowIndex, currentMatchedCellId.ColumnIndex];
                    var upwardsCell = GetUpwardCell(currentMatchedCellId);

                    if (upwardsCell is { })
                        sortedCells.Add(upwardsCell.Id);
                    var shiftTask = ShiftCell(upwardsCell, currentMatchedCellId, currentMatchedCellPosition);

                    matchedIds.Add(currentMatchedCellId);
                    shiftTasks.Add(shiftTask);
                }
            }

            await Task.WhenAll(shiftTasks);
            FindMatches(matchedIds);
        }

        private async Task ShiftCell(ICell cellToShift, CellIndexInGrid currentMatchedCellId,
            Vector3 currentMatchedCellPosition)
        {
            if (cellToShift is null)
            {
                _rowToSpawnQueue[currentMatchedCellId.RowIndex].Enqueue(currentMatchedCellId);
                cellToShift = await SpawnRandomCell(currentMatchedCellId);
                await _rowToSpawnDelayTask[currentMatchedCellId.RowIndex];
                _rowToSpawnDelayTask[currentMatchedCellId.RowIndex] = Task.Delay(SharedData.GameSettings.SpawnDelay);
                _rowToSpawnQueue[currentMatchedCellId.RowIndex].Dequeue();
            }

            await cellToShift.Shift(currentMatchedCellId, currentMatchedCellPosition, ProcessCellShift,
                true);
        }


        ICell GetUpwardCell(CellIndexInGrid indexInGrid)
        {
            var columnIndex = indexInGrid.ColumnIndex - 1;
            ICell cell = default;

            while (columnIndex >= 0 && cell is null)
            {
                cell = _cells[indexInGrid.RowIndex, columnIndex];
                columnIndex--;
            }

            if (cell is { })
                _cells[cell.Id.RowIndex, cell.Id.ColumnIndex] = null;

            return cell;
        }

        public bool IsActive => _swapTask is {};
    }

    public class GridBehaviourController : ControllerBase, IGridBehaviour, ISelectCell
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

        public bool IsActive => _gridInstance.IsActive;

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

            var rowCount = cells.GetRowCount();
            var columnCount = cells.GetColumnCount();
            SharedData.CurrentColumnCount = columnCount;
            SharedData.CurrentRowCount = rowCount;


            var areaHeight = _playingAreaBounds.y * (1 - SharedData.HeightOffset * 2);
            var areaWidth = _playingAreaBounds.x * (1 - SharedData.WidthOffset * 2);

            var approximateGridDimensions = Math.Min(areaHeight, areaWidth);
            var approximateCellDimensions =
                approximateGridDimensions / Math.Max(SharedData.MaxRowSize, SharedData.MaxColumnSize);
            var cellScale = new Vector3(approximateCellDimensions, approximateCellDimensions,
                approximateCellDimensions);
            var halfCell = approximateCellDimensions / 2f;
            var isYEven = columnCount % 2 is 0;
            var isXEven = rowCount % 2 is 0;
            var middleYCell = (int)columnCount / 2;
            var middleXCell = (int)rowCount / 2;
            var firstColumnPosition = GetYPosition(isYEven, halfCell, approximateCellDimensions, middleYCell, 0);

            var cellPool = new CellPool<ICell>(new CellFactory(_cellPrefab, cellScale));
            _gridInstance = new GridInstance((rowCount, columnCount), cellPool,
                _cellSpriteAtlas, _processMatch,
                firstColumnPosition + approximateCellDimensions, firstColumnPosition);

            for (int i = 0; i < rowCount; i++)
            {
                var currentXPos = GetCurrentXPosition(isXEven, halfCell, approximateCellDimensions, middleXCell, i);

                for (int j = 0; j < columnCount; j++)
                {
                    var currentYPos = GetYPosition(isYEven, halfCell, approximateCellDimensions, middleYCell, j);
                    var currentCellPos = new Vector3(currentXPos, currentYPos, 0f);
                    var currentCellType = cells[i, j];

                    _gridInstance.RegisterCell(new CellIndexInGrid { RowIndex = i, ColumnIndex = j },
                        currentCellType, currentCellPos);
                }
            }
        }

        private static float GetCurrentXPosition(bool isEven, float halfCell, float approximateCellDimensions,
            int middleXCell, int index)
        {
            return -((isEven ? halfCell : 0) +
                     (approximateCellDimensions * ((middleXCell - (isEven ? 1 : 0)) - index)));
        }

        private static float GetYPosition(bool isEven, float halfCell, float approximateCellDimensions, int middleYCell,
            int index)
        {
            return (isEven ? halfCell : 0) +
                   (approximateCellDimensions * ((middleYCell - (isEven ? 1 : 0)) - index));
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