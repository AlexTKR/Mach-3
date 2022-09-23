using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [CreateAssetMenu(menuName = "Game Level", fileName = "level_")]
    public class LevelInstance : ScriptableObject
    {
        [SerializeField] private List<RowData> LevelData;

        public CellType[,] CellTypes
        {
            get =>  LevelData?.Count is 0 ? null : GetCells();
            set => SetCells(value);
        }

        private CellType[,] GetCells()
        {
            var rows = LevelData.Count;
            var columns = LevelData[0].Cells.Count;
            CellType[,] cells = new CellType[rows, columns];
            LevelData.Sort((current, next) => current.Index.CompareTo(next.Index));

            for (int i = 0; i < rows; i++)
            {
                var currentCells = LevelData[i].Cells;
                for (int j = 0; j < columns; j++)
                {
                    cells[i, j] = currentCells[j];
                }
            }

            return cells;
        }

        private void SetCells(CellType[,] cells)
        {
            LevelData = new List<RowData>();

            int rows = cells.GetUpperBound(0) + 1; 
            int columns = cells.GetUpperBound(1) + 1;

            for (int i = 0; i < rows; i++)
            {
                var temp = new List<CellType>();

                for (int j = 0; j < columns; j++)
                {
                    temp.Add(cells[i, j]);
                }
                
                LevelData.Add(new RowData(temp, i));
            }
        }
    }
}