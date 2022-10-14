using UnityEngine;

namespace View
{
    public class MapCanvas : CanvasBase
    {
        [SerializeField] private Transform _levelContent;

        public Transform LevelContent => _levelContent;
    }
}
