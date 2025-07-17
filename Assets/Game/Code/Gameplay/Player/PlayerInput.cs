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
                if (TryGetHit(out var hit) && hit.transform.TryGetComponent<IPlayerInteractive>(out var interactive))
                    interactive.Interact();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                if (TryGetHit(out var hit)) 
                    _unitsSelector.Selected.SetDestination(hit.point);
            }
        }

        private bool TryGetHit(out RaycastHit hit)
        {
            var ray = _cameraController.Camera.ScreenPointToRay(Input.mousePosition);
            return Physics.Raycast(ray.origin, ray.direction, out hit) ? hit.transform : false;
        }
    }
}