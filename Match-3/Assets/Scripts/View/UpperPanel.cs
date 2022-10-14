using System.Threading.Tasks;
using Composition;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UtilitiesAndHelpers;
using Zenject;

namespace View
{
    public abstract class UIElementBase : MonoBehaviour
    {
        public bool IsActive => gameObject.activeSelf;
        
        public virtual void Enable()
        {
            gameObject.SetActiveOptimize(true);
        }

        public virtual void Disable()
        {
            gameObject.SetActiveOptimize(false);
        }
    }

    public abstract class PanelBase : UIElementBase, IInitView
    {
        private Controllers.IProcessPanel _processPanel;

        [Inject]
        protected void Construct(Controllers.IProcessPanel processPanel)
        {
            _processPanel = processPanel;
        }

        public virtual Task Initialize()
        {
            return Task.CompletedTask;
        }

        public override void Enable()
        {
            base.Enable();
            _processPanel.ProcessPanelActivated(this);
        }

        public override void Disable()
        {
            base.Disable();
            _processPanel.ProcessPanelActivated(this);
        }
    }

    public class UpperPanel : PanelBase
    {
        [SerializeField] private TextMeshProUGUI _movesCount;
        [SerializeField] private TextMeshProUGUI _levelNumber;
        [SerializeField] private Button menuButton;
        [SerializeField] private Transform _goalContent;

        private PausePanel _pausePanel;

        public Transform GoalContent => _goalContent;

        [Inject]
        private void Construct(PausePanel pausePanel)
        {
            _pausePanel = pausePanel;
        }

        public override Task Initialize()
        {
            menuButton.onClick.AddListener(() => { _pausePanel.Enable(); });
            return base.Initialize();
        }

        public void SetMovesCount(int count)
        {
            _movesCount.text = count.ToString();
        }

        public void SetLevelNumber(int levelNumber)
        {
            _levelNumber.text = $"Level {levelNumber}";
        }
    }
}