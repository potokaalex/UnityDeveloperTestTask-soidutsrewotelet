using System;
using Game.Code.Gameplay.Player;
using UniRx;
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
        public float AttackRange; //1-5
        public float BodyRadius = 0.125f;
        public float PositionVelocity = 5;
        public NavMeshObstacle NavMeshObstacle;
        private PlayerController _playerController;
        private UnitsSelector _selector;
        private NavMeshPath _path;
        private bool _moving;
        private int _currentCornerIndex;

        public bool IsEnemy => _playerController.Team != Team.Value;

        public NetworkVariable<TeamType> Team { get; } = new(); //use spawn payload ?

        public ReactiveProperty<Vector3[]> PathPoints { get; } = new();

        public ReactiveProperty<bool> IsSelect { get; } = new();

        public bool IsDestinationSet => PathPoints.Value != null && PathPoints.Value.Length > 0;

        [Inject]
        public void Construct(PlayerController playerController, UnitsSelector selector)
        {
            _playerController = playerController;
            _selector = selector;
        }

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

        public void OnSelect() => OnSelectServerRpc();

        public void OnUnSelect() => OnUnSelectServerRpc();

        public void SetDestination(Vector3 point) => SetDestinationServerRpc(point);

        public void MoveDestination() => MoveDestinationServerRpc();

        public void ClearDestination() => ClearDestinationServerRpc();

        [ServerRpc(RequireOwnership = false)]
        private void OnSelectServerRpc(ServerRpcParams rpcParams = default)
        {
            NavMeshObstacle.enabled = false;
            SetIsSelectClientRpc(true,
                new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new[] { rpcParams.Receive.SenderClientId } } });
        }

        [ServerRpc(RequireOwnership = false)]
        private void OnUnSelectServerRpc(ServerRpcParams rpcParams = default)
        {
            NavMeshObstacle.enabled = true;
            SetIsSelectClientRpc(false,
                new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new[] { rpcParams.Receive.SenderClientId } } });
        }

        [ClientRpc]
        private void SetIsSelectClientRpc(bool isSelect, ClientRpcParams rpcParams = default) => IsSelect.Value = isSelect;

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
        private void SetPathClientRpc(Vector3[] path, ClientRpcParams rpcParams = default) => PathPoints.Value = path;

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
            ClearDestinationClientRpc(new ClientRpcParams
                { Send = new ClientRpcSendParams { TargetClientIds = new[] { rpcParams.Receive.SenderClientId } } });
        }

        [ClientRpc]
        private void ClearDestinationClientRpc(ClientRpcParams rpcParams = default) => PathPoints.Value = Array.Empty<Vector3>();

        //public bool IsInRange(Vector3 center, float radius) =>
        //    MathExtensions.IsCirclesIntersect(center, radius, transform.position, BodyRadius); //логика
    }
}