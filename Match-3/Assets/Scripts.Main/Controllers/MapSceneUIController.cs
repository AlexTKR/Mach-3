using System.Threading.Tasks;
using Scripts.ViewModel.PanelViewModels;
using Zenject;

namespace Scripts.Main.Controllers
{
    public interface IloadMapItem
    {
        void LoadMapItem(int levelNumber, bool isActive);
    }

    public class MapSceneUIController : ControllerBase, IloadMapItem
    {
        private ISelectLevel _selectLevel;
        private IMapPanelBehaviour _mapPanelBehaviour;

        [Inject]
        void Construct(ISelectLevel selectLevel, IMapPanelBehaviour mapPanelBehaviour)
        {
            _selectLevel = selectLevel;
            _mapPanelBehaviour = mapPanelBehaviour;
        }

        public void LoadMapItem(int levelNumber, bool isActive)
        {
            _mapPanelBehaviour.LoadMapItem(levelNumber, isActive, ProcessLevelSelected);
        }

        private void ProcessLevelSelected(int levelNumber)
        {
            _selectLevel.SelectLevel(levelNumber);
        }
    }
}