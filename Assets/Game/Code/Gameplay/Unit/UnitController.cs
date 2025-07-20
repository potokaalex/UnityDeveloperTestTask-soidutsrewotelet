using Game.Code.Core.Network;
using Game.Code.Gameplay.Unit.View;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay.Unit
{
    public class UnitController : NetworkBehaviour
    {
        public UnitModel Model;
        public UnitView View;
        public UnitAttackController AttackController;
        public UnitHelper Helper;
        public UnitSelectionController Selection;
        public UnitMovementController Movement;
        private UnitsContainer _container;

        public int Type => Model.Type;

        public ulong Id => Model.Id;

        public bool IsDestinationValid => Helper.IsDestinationValid;

        public TeamType Team => Model.Team;

        public bool IsEnemy => Helper.IsEnemy;

        [Inject]
        public void Construct(UnitsContainer container) => _container = container;

        public void Initialize(Vector3 position, TeamType team)
        {
            transform.position = position;
            Model.TeamNV.Initialize(this, team);
        }

        public override void OnNetworkSpawn()
        {
            _container.Add(this);
            View.Setup(Model.Team);
        }

        public override void OnNetworkDespawn() => _container.Remove(this);

        public void Select() => Selection.Select(this);

        public void OnSelect()
        {
            Selection.OnSelectServerRpc();
            View.ViewUnSelect();
            Movement.Setup(this);
            AttackController.Setup(this);
        }

        public void OnUnSelect()
        {
            Selection.OnUnSelectServerRpc();
            View.ViewUnSelect();
            Movement.Clear();
            AttackController.Clear();
        }

        public void SetDestination(Vector3 point) => Movement.SetDestinationServerRpc(point);

        public void ClearDestination() => Movement.ClearDestinationServerRpc();

        public void MoveDestination() => Movement.MoveDestinationServerRpc();

        public void Attack(UnitController unit) => AttackController.Attack(unit);

        public bool IsInRange(Vector3 center, float radius) => Helper.IsInRange(center, radius);
    }
}