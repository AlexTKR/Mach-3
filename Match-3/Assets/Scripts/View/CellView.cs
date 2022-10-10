using System.Threading.Tasks;
using Controllers;
using UnityEngine;
using UtilitiesAndHelpers;

namespace View
{
    public interface ICellView : ICellIdentifier,  IWorldCellPosition,
        IHighlightable, IMatch
    {
        void SetData(Sprite sprite, CellIndexInGrid id, Vector3? position = null);
        Task Shift(CellIndexInGrid CellIndexInGrid, Vector3 position);
    }

    public class CellView : MonoBehaviour, ICellView
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Color _idleColor;
        [SerializeField] private Color _selectedColor;
        
        public CellIndexInGrid Id { get; private set; }
        
        public Vector3? CellPosition => transform.position;

        public async Task Shift(CellIndexInGrid id, Vector3 position)
        {
            Id = id;
            transform.position = position;
        }

        public void SetData(Sprite sprite, CellIndexInGrid id, Vector3? position = null)
        {
            Id = id;
            _spriteRenderer.sprite = sprite;
            if (position is { } pos)
                transform.position = pos;
            
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