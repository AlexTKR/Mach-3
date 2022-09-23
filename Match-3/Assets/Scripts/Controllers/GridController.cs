using System;
using System.Threading.Tasks;
using Level;
using UnityEngine;
using UnityEngine.U2D;
using UtilitiesAndHelpers;
using View;
using Zenject;

namespace Controllers
{
    public interface IGrid
    {
        void GenerateGrid(CellType[,] cells);
    }

    public class GridController : ControllerBase, IGrid
    {
        private IGetLevel _getLevel;
        private MainCamera _mainCamera;
        private SpriteAtlas _cellSpriteAtlas;
        private CellView _cellPrefab;
        (float widht, float height) _screenData = (Screen.width, Screen.height);
        private Vector3 _playingAreaBounds;
        private Vector3 _screenBounds;
        
        [Inject]
        void Construct(MainCamera mainCamera, IGetLevel getLevel, IGetCellAtlas getCellAtlas,
            IGetCell getCell)
        {
            _getLevel = getLevel;
            _mainCamera = mainCamera;
            _cellSpriteAtlas = getCellAtlas.GetCellAtlas();
            _cellPrefab = getCell.GetCellPrefab();
        }
        
        public override Task Initialize()
        {
            _screenBounds = _mainCamera.Camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,
                _mainCamera.Camera.transform.position.z));
            _playingAreaBounds =
                new Vector3(Math.Abs(_screenBounds.x * 2), Math.Abs(_screenBounds.y * 2), _screenBounds.z);
            var firstLevelCells = _getLevel.GetLevel(1);
            GenerateGrid(firstLevelCells);
            return base.Initialize();
        }
        
        public void GenerateGrid(CellType[,] cells)
        {
            if (cells is null)
                return;

            var rowCount = cells.GetRowCount();
            var columnCount = cells.GetColumnCount();

            var areaHeight = _playingAreaBounds.y * (1 - SharedData.HeightOffset * 2);
            var areaWidth = _playingAreaBounds.x * (1 - SharedData.WidthOffset * 2);

            var approximateGridDimensions = Math.Min(areaHeight, areaWidth);
            var approximateCellDimensions =
                approximateGridDimensions / Math.Max(SharedData.MaxRowSize, SharedData.MaxColumnSize);

            var halfCell = approximateCellDimensions / 2f;
            var halfGrid = approximateGridDimensions / 2f;
            var cellPositions = new Vector3[rowCount, columnCount];

            for (int i = 0; i < rowCount; i++)
            {
                var currentXPos = (-halfGrid + halfCell) + approximateCellDimensions * i;
                for (int j = 0; j < columnCount; j++)
                {
                    var currentYPos = (halfGrid - halfCell) - approximateCellDimensions * j;
                    cellPositions[i, j] = new Vector3(currentXPos, currentYPos, 0f);
                    var cell = MonoBehaviour.Instantiate(_cellPrefab, cellPositions[i, j], Quaternion.identity);
                    cell.transform.localScale = new Vector3(approximateCellDimensions, approximateCellDimensions,
                        approximateCellDimensions);
                    var currentCellType = cells[i, j];
                    cell.SetSprite(currentCellType is CellType.Empty
                        ? null
                        : _cellSpriteAtlas.GetSprite(Helpers.CellTypeToName[currentCellType]));
                }
            }
        }
    }
}
