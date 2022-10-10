using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level
{
    [Serializable]
    public enum CellType
    {
        Empty,
        Red,
        Yellow,
        Blue,
        Purple,
        Green
    }
    
    [Serializable]
    public class RowData
    {
        [SerializeField] private int _index;
        [SerializeField] private List<CellType> _cells;

        public int Index => _index;
        public IList<CellType> Cells => _cells;

        public RowData(IList<CellType> cells, int index)
        {
            _index = index;
            _cells = cells.ToList();
        }
    }

    public class LevelConstructor : MonoBehaviour
    {
        public Vector2Int Dimensions;
        public LevelInstance currentLevelInstance;
    }
}