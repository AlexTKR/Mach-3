using Scripts.CommonExtensions.Scripts;
using Scripts.Main.Controllers;
using Scripts.Main.View;
using UnityEngine;

namespace Scripts.Main.UtilitiesAndHelpers
{
    public interface IFactory<T>
    {
        T Get();
    }

    public class CellFactory : IFactory<ICell>
    {
        private CellView _cellPrefab;
        private Vector3 _cellScale;

        public CellFactory(CellView cellPrefab, Vector3 cellScale)
        {
            _cellPrefab = cellPrefab;
            _cellScale = cellScale;
        }

        public ICell Get()
        {
            var cellView = MonoBehaviour.Instantiate(_cellPrefab, Vector3.zero, Quaternion.identity);
            cellView.transform.localScale = _cellScale;
            cellView.gameObject.SetActiveOptimize(false);
            return new Cell(cellView);
        }
    }
}