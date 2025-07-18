using System;
using System.Collections.Generic;
using System.Linq;
using Game.Code.Core;
using Game.Code.Gameplay.Player;
using Game.Code.Gameplay.Unit.View;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Code.Gameplay.Unit
{
    public class UnitController : NetworkBehaviour, IPlayerInteractive
    {
        public int Type;
        public float Speed;
        public float AttackRadius;
        public float BodyRadius = 0.125f;
        public float PositionVelocity = 5;
        public UnitView View;
        public NavMeshObstacle NavMeshObstacle;
        private PlayerController _playerController;
        private UnitsSelector _selector;
        private UnitsContainer _container;
        private NavMeshPath _path;
        private bool _moving;
        private int _currentCornerIndex;

        public NetworkVariable<TeamType> Team { get; } = new(); //use spawn payload ?

        public NetworkVariable<int> Id { get; } = new();

        public Vector3[] PathPoints { get; private set; }

        public bool IsEnemy => _playerController.Team != Team.Value;

        public bool IsDestinationSet => PathPoints != null && PathPoints.Length > 0;

        public float FullAttackRadius => AttackRadius + BodyRadius;

        [Inject]
        public void Construct(PlayerController playerController, UnitsSelector selector, UnitsContainer container)
        {
            _playerController = playerController;
            _selector = selector;
            _container = container;
        }

        public override void OnNetworkSpawn() => _container.Add(this, Team.Value);

        public void Update()
        {
            if (!IsServer || !_moving || _path.corners.Length == 0 || _currentCornerIndex >= _path.corners.Length)
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
            if (!IsEnemy)
                _selector.Select(this);
        }

        public void OnSelect()
        {
            OnSelectServerRpc();
            View.ViewSelect(transform.position, Speed);
            CalculateAttack(transform.position);
        }

        public void OnUnSelect()
        {
            OnUnSelectServerRpc();
            using var d = GetAllEnemies(out var list);
            View.ViewUnSelect(list);
        }

        public void SetDestination(Vector3 point) => SetDestinationServerRpc(point);

        public void MoveDestination() => MoveDestinationServerRpc();

        public void ClearDestination() => ClearDestinationServerRpc();

        [ServerRpc(RequireOwnership = false)]
        private void OnSelectServerRpc() => NavMeshObstacle.enabled = false;

        [ServerRpc(RequireOwnership = false)]
        private void OnUnSelectServerRpc() => NavMeshObstacle.enabled = true;

        [ServerRpc(RequireOwnership = false)]
        private void SetDestinationServerRpc(Vector3 point, ServerRpcParams rpcParams = default)
        {
            var path = Array.Empty<Vector3>();

            if (Vector3.Distance(transform.position, point) <= Speed)
            {
                _path ??= new();
                if (NavMesh.CalculatePath(transform.position, point, NavMesh.AllAreas, _path))
                    path = _path.corners;
            }

            SetPathClientRpc(path,
                new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new[] { rpcParams.Receive.SenderClientId } } });
        }

        [ClientRpc]
        private void SetPathClientRpc(Vector3[] path, ClientRpcParams rpcParams = default)
        {
            PathPoints = path;
            if (_selector.Selected == this)
                View.OnDestinationChanged(IsDestinationSet, PathPoints);
            CalculateAttack(PathPoints.Length > 0 ? PathPoints[^1] : transform.position);
        }

        [ServerRpc(RequireOwnership = false)]
        private void MoveDestinationServerRpc(ServerRpcParams rpcParams = default)
        {
            _moving = true;
            OnMoveDestinationClientRpc(
                new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new[] { rpcParams.Receive.SenderClientId } } });
        }

        [ClientRpc]
        private void OnMoveDestinationClientRpc(ClientRpcParams rpcParams = default) => _selector.UnSelect(this);

        [ServerRpc(RequireOwnership = false)]
        private void ClearDestinationServerRpc(ServerRpcParams rpcParams = default)
        {
            _path.ClearCorners();
            ClearDestinationClientRpc(new ClientRpcParams
                { Send = new ClientRpcSendParams { TargetClientIds = new[] { rpcParams.Receive.SenderClientId } } });
        }

        [ClientRpc]
        private void ClearDestinationClientRpc(ClientRpcParams rpcParams = default)
        {
            PathPoints = Array.Empty<Vector3>();
            if (_selector.Selected == this)
                View.OnDestinationChanged(IsDestinationSet, PathPoints);
            CalculateAttack(transform.position);
        }

        private void CalculateAttack(Vector3 point, ServerRpcParams rpcParams = default)
        {
            if (_selector.Selected == this)
            {
                using var d = GetUnitsForAttack(point, out var forAttack);
                using var d1 = GetAllEnemies(out var enemies);
                View.ViewAttack(point, FullAttackRadius, forAttack, enemies);
            }
        }

        private IDisposable GetUnitsForAttack(Vector3 point, out List<UnitController> outList)
        {
            using var d = UnityEngine.Pool.ListPool<UnitController>.Get(out var units);
            _container.Get(units);

            var result = UnityEngine.Pool.ListPool<UnitController>.Get(out outList);
            foreach (var unit in units)
                if (unit.IsEnemy && unit.IsInRange(point, FullAttackRadius))
                    outList.Add(unit);

            return result;
        }

        private IDisposable GetAllEnemies(out List<UnitController> outList)
        {
            using var d = UnityEngine.Pool.ListPool<UnitController>.Get(out var units);
            _container.Get(units);

            var result = UnityEngine.Pool.ListPool<UnitController>.Get(out outList);
            foreach (var unit in units)
                if (unit.IsEnemy)
                    outList.Add(unit);
            return result;
        }

        private bool IsInRange(Vector3 center, float radius) =>
            MathExtensions.IsCirclesIntersect(center, radius, transform.position, BodyRadius);
    }
}