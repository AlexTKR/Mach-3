using System.Threading.Tasks;
using Controllers;
using DG.Tweening;
using UnityEngine;
using UtilitiesAndHelpers;

namespace View
{
    public interface ICellView : ICellIdentifier, IWorldCellPosition,
        IHighlightable, IMatch, IMoveCell
    {
        void SetData(Sprite sprite, CellIndexInGrid id, Vector3 position);
        Task Shift(CellIndexInGrid cellIndexInGrid, Vector3 position, bool onMatchShift = false);
    }

    public class CellView : MonoBehaviour, ICellView
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _idleColor;
        [SerializeField] private Color _selectedColor;

        public CellIndexInGrid Id { get; private set; }

        public Vector3? CellPosition => transform.position;

        public async Task Shift(CellIndexInGrid cellIndexInGrid, Vector3 position, bool onMatchShift = false)
        {
            Id = cellIndexInGrid;
            var d = GetDuration(position);
            var tw = transform.DOMove(position, GetDuration(position));

            if (onMatchShift)
                tw.SetEase(SharedData.GameSettings.ShiftCellCurve);

            await DOTween.Sequence().Append(tw).AsyncWaitForCompletion();
        }

        public async Task Move(Vector3 position)
        {
            await transform.DOMove(position, GetDuration(position)).AsyncWaitForCompletion();
        }

        private float GetDuration(Vector3 position)
        {
            return (position - transform.position).magnitude / SharedData.GameSettings.CellShiftSpeed;
        }

        public void SetData(Sprite sprite, CellIndexInGrid id, Vector3 position)
        {
            Id = id;
            _spriteRenderer.sprite = sprite;
            transform.position = position;

            gameObject.SetActiveOptimize(true);
        }

        public void Highlight(bool status)
        {
            _spriteRenderer.color = status ? _selectedColor : _idleColor;
        }

        public void Match()
        {
            gameObject.SetActive(false);
        }
    }
}