using System.Collections.Generic;
using Scripts.CommonBehaviours;
using UnityEngine;

namespace Scripts.Main.Level
{
    [CreateAssetMenu(menuName = "Game Level", fileName = "level_")]
    public class LevelInstance : ScriptableObject
    {
        [SerializeField] private List<RowData> LevelData;

        public CellType[,] CellTypes
        {
            get => LevelData?.Count is 0 ? null : GetCells();
            set => SetCells(value);
        }

        private CellType[,] GetCells()
        {
            var columns = LevelData.Count;
            var rows  = LevelData[0].Cells.Count;
            var cells = new CellType[rows, columns];
            LevelData.Sort((current, next) => current.Index.CompareTo(next.Index));

            for (int i = 0; i < columns; i++)
            {
                var currentCells = LevelData[i].Cells;
                for (int j = 0; j < rows; j++)
                {
                    cells[j, i] = currentCells[j];
                }
            }

            return cells;
        }

        private void SetCells(CellType[,] cells)
        {
            LevelData = new List<RowData>();

            int rows = cells.GetUpperBound(0) + 1;
            int columns = cells.GetUpperBound(1) + 1;

            for (int i = 0; i < columns; i++)
            {
                var temp = new List<CellType>();

                for (int j = 0; j < rows; j++)
                {
                    temp.Add(cells[j, i]);
                }

                LevelData.Add(new RowData(temp, i));
            }
        }
    }
}