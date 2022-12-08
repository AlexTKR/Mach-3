using System;
using System.Threading.Tasks;
using Scripts.CommonBehaviours;
using Scripts.CommonExtensions.Scripts;
using Scripts.Main.Loadable;
using Scripts.Main.UtilitiesAndHelpers;
using Scripts.Main.View;
using UnityEngine;
using UnityEngine.U2D;
using View;
using Zenject;

namespace Scripts.Main.Controllers
{
    public class GridController : ControllerBase, IGridBehaviour, ISelectCell
    {
        private IGrid _gridInstance;
        private IProcessMove _processMove;
        private IProcessMatch _processMatch;
        private MainCamera _mainCamera;
        private SpriteAtlas _cellSpriteAtlas;
        private CellView _cellPrefab;
        private Vector3 _playingAreaBounds;
        private Vector3 _screenBounds;
        private Task<bool> _selectTask;

        [Inject]
        void Construct(MainCamera mainCamera, IGetCellAtlas getCellAtlas,
            IGetCell getCell, IProcessMove processMove, IProcessMatch processMatch
            , IGetLevelSettings getLevelSettings)
        {
            _mainCamera = mainCamera;
            _cellSpriteAtlas = getCellAtlas.GetCellAtlas().Load(runAsync:false).Result;
            _cellPrefab = getCell.GetCellPrefab().Load(runAsync:false).Result;
            _processMove = processMove;
            _processMatch = processMatch;
        }

        public bool IsActive => _gridInstance.IsActive;

        public override Task Init()
        {
            _screenBounds = _mainCamera.Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
                _mainCamera.Camera.transform.position.z));
            _playingAreaBounds =
                new Vector3(Math.Abs(_screenBounds.x * 2), Math.Abs(_screenBounds.y * 2), _screenBounds.z);
            return base.Init();
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
                firstColumnPosition + approximateCellDimensions);

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