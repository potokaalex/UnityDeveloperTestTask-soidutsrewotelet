using Game.Code.Gameplay.Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Game.Code.Gameplay.Player
{
    public class PlayerInput : ITickable
    {
        private readonly CameraController _cameraController;
        private readonly UnitsSelector _unitsSelector;
        private float _lastClickTime;
        private Vector3 _lastClickPosition;

        public PlayerInput(CameraController cameraController, UnitsSelector unitsSelector)
        {
            _cameraController = cameraController;
            _unitsSelector = unitsSelector;
        }

        public void Tick()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            if (Input.GetMouseButtonDown(0))
            {
                if (_unitsSelector.HasSelected && _unitsSelector.Selected.IsDestinationValid)
                    _unitsSelector.Selected.ClearDestination();

                if (TryGetHit(out var hit) && hit.transform.TryGetComponent<UnitController>(out var unit))
                    unit.Select();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (TryGetHit(out var hit) && _unitsSelector.HasSelected)
                {
                    if (hit.transform.TryGetComponent<UnitController>(out var unit))
                        _unitsSelector.Selected.Attack(unit);
                    else
                    {
                        _unitsSelector.Selected.SetDestination(hit.point);
                        if (IsDoubleClick())
                            _unitsSelector.Selected.MoveDestination();
                    }
                }
            }
        }

        private bool IsDoubleClick()
        {
            var timeSinceLastClick = Time.time - _lastClickTime;
            var mousePositionDelta = Vector3.Distance(_lastClickPosition, Input.mousePosition);
            _lastClickTime = Time.time;
            _lastClickPosition = Input.mousePosition;
            var doubleClickThreshold = 0.3f;
            return timeSinceLastClick <= doubleClickThreshold && mousePositionDelta < 0.05f;
        }

        private bool TryGetHit(out RaycastHit hit)
        {
            var ray = _cameraController.Camera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray.origin, ray.direction, out hit) ? hit.transform : false;
        }
    }
}