using System;
using Controllers;
using Level;
using UnityEngine;
using UnityEngine.U2D;
using View;

namespace UtilitiesAndHelpers
{
    public interface IFactory<T>
    {
        T Get();
    }

    public class CellFactory : IFactory<ICell>
    {
        private CellView _cellPrefab;

        public CellFactory(CellView cellPrefab)
        {
            _cellPrefab = cellPrefab;
        }

        public ICell Get()
        {
            var cellView = MonoBehaviour.Instantiate(_cellPrefab, Vector3.zero, Quaternion.identity);
            cellView.gameObject.SetActiveOptimize(false);
            return new Cell(cellView);
        }
    }
}