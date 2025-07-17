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
        public NavMeshObstacle NavMeshObstacle;
        public GameObject CanBeAttackedIndicator;
        public int Type;
        public float MoveRange;
        public float AttackRange;
        public float PositionVelocity;
        public float BodyRadius;
        private NavMeshPath _path;
        private UnitsSelector _selector;
        private UnitRangeView _rangeView;
        private UnitPathView _pathView;
        private UnitAttackController _attackController;
        private bool _moving;
        private int _currentCornerIndex;

        public bool DestinationSet { get; private set; }

        public bool IsEnemy => !IsOwner;

        [Inject]
        public void Construct(UnitsSelector selector, UnitRangeView rangeView, UnitPathView unitPathView, UnitAttackController attackController)
        {
            _attackController = attackController;
            _pathView = unitPathView;
            _rangeView = rangeView;
            _selector = selector;
        }

        public void Update()
        {
            if (!IsOwner || !_moving || _path.corners.Length == 0 || _currentCornerIndex >= _path.corners.Length)
                return;

            var target = _path.corners[_currentCornerIndex];
            var direction = (target - transform.position).normalized;

            var offset = PositionVelocity * Time.deltaTime;
            var distance = Vector3.Distance(transform.position, target);

            if (offset >= distance)
            {
                transform.position = target;
                _currentCornerIndex++;

                if (_currentCornerIndex >= _path.corners.Length)
                {
                    _moving = false;
                    _currentCornerIndex = 0;
                }
            }
            else
                transform.position += direction * offset;
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
            _attackController.Set(transform.position, AttackRange);
            NavMeshObstacle.enabled = false;
        }

        public void OnUnSelect()
        {
            SelectionIndicator.SetActive(false);
            _rangeView.ClearMove();
            _attackController.Clear();
            _pathView.Clear();
            NavMeshObstacle.enabled = true;
        }

        public void SetDestination(Vector3 point)
        {
            if (TryGetPath(point))
            {
                DestinationSet = true;
                _pathView.View(_path);
                _attackController.Set(point, AttackRange);
            }
            else
                DestinationSet = false;
        }

        public void ClearDestination()
        {
            DestinationSet = false;
            _pathView.Clear();
            _attackController.Set(transform.position, AttackRange);
        }

        public void MoveDestination()
        {
            if (DestinationSet)
            {
                _moving = true;
                _selector.UnSelect(this);
            }
        }

        public bool IsInRange(Vector3 center, float radius) => MathExtensions.IsCirclesIntersect(center, radius, transform.position, BodyRadius);

        public void ShowCanBeAttacked() => CanBeAttackedIndicator.SetActive(true);

        public void HideCanBeAttacked() => CanBeAttackedIndicator.SetActive(false);

        private bool TryGetPath(Vector3 toPoint)
        {
            _path ??= new();

            if (Vector3.Distance(transform.position, toPoint) > MoveRange)
                return false;
            return NavMesh.CalculatePath(transform.position, toPoint, NavMesh.AllAreas, _path);
        }
    }
}