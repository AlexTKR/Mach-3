using UnityEngine;

namespace View
{
    public class MapCanvas : PanelBase
    {
        [SerializeField] private Transform _levelContent;

        public Transform LevelContent => _levelContent;
    }
}
