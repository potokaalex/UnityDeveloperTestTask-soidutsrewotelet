using Game.Code.Gameplay.Player;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Code.Gameplay.Unit
{
    public class UnitController : NetworkBehaviour, IPlayerInteractive
    {
        public GameObject SelectionIndicator;
        public NavMeshAgent NavMeshAgent;
        public int Type;
        public float MoveRange;
        public float AttackRange;
        private UnitsSelector _selector;
        private UnitRangeView _rangeView;
        private UnitPathView _pathView;

        [Inject]
        public void Construct(UnitsSelector selector, UnitRangeView unitRangeView, UnitPathView unitPathView)
        {
            _pathView = unitPathView;
            _rangeView = unitRangeView;
            _selector = selector;
        }

        public void Interact()
        {
            if (IsOwner)
                _selector.Select(this);
        }

        public void OnSelect()
        {
            SelectionIndicator.SetActive(true);
            _rangeView.ViewMove(MoveRange, transform.position);
            _rangeView.ViewAttack(AttackRange, transform.position);
        }

        public void OnUnSelect()
        {
            SelectionIndicator.SetActive(false);
            _rangeView.ClearMove();
            _rangeView.ClearAttack();
            _pathView.Clear();
        }

        public void SetDestination(Vector3 point)
        {
            var path = new NavMeshPath();
            NavMeshAgent.CalculatePath(point, path);
            _pathView.View(path);
            _rangeView.ViewAttack(AttackRange, point);
            _rangeView.ClearMove();
        }
    }
}