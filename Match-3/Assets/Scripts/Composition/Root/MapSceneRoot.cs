using Controllers;
using Root;
using Zenject;

namespace Composition.Root
{
    public class MapSceneRoot : RootBase
    {
        private IMapController _mapController; 
        
        [Inject]
        private void Construct(IMapController mapController)
        {
            _mapController = mapController;
        }
        
        protected override void OnStart()
        {
            _mapController.LoadMap();
        }
    }
}
