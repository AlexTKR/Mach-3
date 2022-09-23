using System.Threading.Tasks;
using UtilitiesAndHelpers;
using View;
using Zenject;

namespace Controllers
{
    public interface IGetMainCamera 
    {
        MainCamera GetCamera { get; }
    }

    public class CameraController : IInitControllers , IGetMainCamera
    {
        private MainCamera _mainCamera;
        private IGetMainCamera _getMainCameraImplementation;

        [Inject]
        private void Construct(MainCamera mainCamera)
        {
            _mainCamera = mainCamera;
        }


        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public MainCamera GetCamera => _mainCamera;
    }
}
