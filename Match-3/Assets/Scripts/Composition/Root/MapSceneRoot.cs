using Controllers;
using Root;
using Zenject;

namespace Composition.Root
{
    public class MapSceneRoot : RootInitiator
    {
        private IMapController _mapController; 
        
        [Inject]
        private void Construct(IMapController mapController)
        {
            _mapController = mapController;
        }

        protected override void Start()
        {
            base.Start();
            _mapController.LoadMap();
        }
    }
}
