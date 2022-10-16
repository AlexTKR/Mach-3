using UnityEngine;
using View;
using Zenject;

namespace Controllers
{
    public class InputController : ControllerBase
    {
        private MainCamera _mainCamera;
        private ISelectCell _selectCell;
        private IAnyPanelActive _anyPanelActive;

        #region fields

        private float _rayDistance = 100;

        #endregion


        [Inject]
        private void Construct(MainCamera mainCamera, ISelectCell selectCell,
            IAnyPanelActive anyPanelActive)
        {
            _mainCamera = mainCamera;
            _selectCell = selectCell;
            _anyPanelActive = anyPanelActive;
        }

        public override void Tick()
        {
            base.Tick();
            ReadInput();
        }

        private void ReadInput()
        {
            if(_anyPanelActive.IsAnyPanelActive)
                return;
            
            if (!Input.GetKeyDown(KeyCode.Mouse0))
                return;

            var camera = _mainCamera.Camera;
            var touchPos = Input.mousePosition;
            var ray = camera.ScreenPointToRay(touchPos);
            if (!Physics.Raycast(ray, out var hit, _rayDistance))
                return;

            var cellView = hit.collider.transform.GetComponent<ICellView>();
            if (cellView is null)
                return;

            _selectCell.SelectCell(cellView.Id);
        }
    }
}