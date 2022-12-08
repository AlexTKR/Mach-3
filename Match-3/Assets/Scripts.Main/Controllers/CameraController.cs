using View;
using Zenject;

namespace Scripts.Main.Controllers
{
    public interface IGetMainCamera 
    {
        MainCamera GetCamera { get; }
    }

    public class CameraController : ControllerBase , IGetMainCamera
    {
        private MainCamera _mainCamera;
        private IGetMainCamera _getMainCameraImplementation;

        [Inject]
        private void Construct(MainCamera mainCamera)
        {
            _mainCamera = mainCamera;
        }
        
        public MainCamera GetCamera => _mainCamera;
    }
}
