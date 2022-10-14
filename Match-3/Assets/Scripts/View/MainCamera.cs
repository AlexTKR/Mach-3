using UnityEngine;

namespace View
{
    public class MainCamera : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        public Camera Camera => _camera;
    }
}
