using View;

namespace Controllers
{
    public interface ISetPanelProcessor
    {
        void SetProcessor(IPanelProcessor panelProcessor);
    }

    public interface IAnyPanelActive
    {
        bool IsAnyPanelActive { get; }
    }

    public interface IProcessPanel :  ISetPanelProcessor, IAnyPanelActive
    {
        void ProcessPanelActivated(PanelBase panel);
        
        void ProcessSceneChanged();
    }

    public class ProcessPanelController : IProcessPanel
    {
        private View.IPanelProcessor _panelProcessor;
        private PanelBase _currentActivePanel;

        public bool IsAnyPanelActive => _currentActivePanel is { IsActive: true };

        public void ProcessPanelActivated(PanelBase panel)
        {
            if (_panelProcessor is null)
                return;
            
            _panelProcessor.ProcessPanelActivated(panel.IsActive);

            if(_currentActivePanel == panel)
                return;
            
            if (_currentActivePanel is { })
                _currentActivePanel.Disable();

            _currentActivePanel = panel;
        }

        public void SetProcessor(View.IPanelProcessor panelProcessor)
        {
            _panelProcessor = panelProcessor;
        }

        public void ProcessSceneChanged()
        {
            _panelProcessor = null;
            _currentActivePanel = null;
        }
    }
}