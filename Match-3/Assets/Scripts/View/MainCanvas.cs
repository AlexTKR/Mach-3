using UnityEngine;
using UtilitiesAndHelpers;

namespace View
{
    public interface IPanelProcessor
    {
        void ProcessPanelActivated(bool status);
    }

    public abstract class CanvasBase : PanelBase, IPanelProcessor
    {
        [SerializeField] protected GameObject _blockingPanel;
        
        public virtual void ProcessPanelActivated(bool status)
        {
            _blockingPanel.SetActiveOptimize(status);
        }
    }

    public class MainCanvas : CanvasBase
    {
        
    }
}
