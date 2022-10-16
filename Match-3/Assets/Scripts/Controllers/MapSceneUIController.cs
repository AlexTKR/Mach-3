using System.Threading.Tasks;
using UnityEngine;
using View;
using Zenject;

namespace Controllers
{
    public interface ILevelPanel
    {
        void LoadPanel(int levelNumber, bool isActive);
    }

    public class MapSceneUIController : ControllerBase, ILevelPanel
    {
        private ISelectLevel _selectLevel;
        private IGetLevelPanel _getLevelPanel;
        private LevelPanel _leftPanelPrefab;
        private LevelPanel _rightPanelPrefab;
        private MapCanvas _mapCanvas;
        private bool isLastLeft;

        [Inject]
        void Construct(IGetLevelPanel getLevelPanel,  ISelectLevel selectLevel ,MapCanvas mapCanvas)
        {
            _getLevelPanel = getLevelPanel;
            _selectLevel = selectLevel;
            _mapCanvas = mapCanvas;
        }

        public override Task Initialize()
        {
            _leftPanelPrefab = _getLevelPanel.GetLeft();
            _rightPanelPrefab = _getLevelPanel.GetRight();
            return base.Initialize();
        }

        public void LoadPanel(int levelNumber, bool isActive)
        {
            var currentPanel = MonoBehaviour.Instantiate(isLastLeft ? _rightPanelPrefab : _leftPanelPrefab,
                _mapCanvas.LevelContent);
            isLastLeft = !isLastLeft;
            currentPanel.transform.SetSiblingIndex(0);
            currentPanel.Initialize(levelNumber, isActive, ProcessLevelSelected);
        }

        private void ProcessLevelSelected(int levelNumber)
        {
            _selectLevel.SelectLevel(levelNumber);
        }
    }
}