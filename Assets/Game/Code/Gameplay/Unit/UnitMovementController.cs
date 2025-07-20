using System;
using Game.Code.Core.Network;
using Game.Code.Gameplay.Player;
using Game.Code.Gameplay.Unit.View;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Game.Code.Gameplay.Unit
{
    public class UnitMovementController : NetworkBehaviour
    {
        public UnitModel Model;
        public UnitView View;
        public UnitHelper Helper;
        public UnitAttackController Attack;
        private PlayersContainer _playersContainer;
        private UnitsSelector _selector;
        private UnitController _unit;
        private bool _moving;
        private int _currentCornerIndex;
        private IPlayerProvider _playerProvider;

        [Inject]
        public void Construct(PlayersContainer playersContainer, UnitsSelector selector, IPlayerProvider playerProvider)
        {
            _playersContainer = playersContainer;
            _selector = selector;
            _playerProvider = playerProvider;
        }

        public void Update()
        {
            if (!IsServer || !_moving || Model.PathPoints.Length == 0 || _currentCornerIndex >= Model.PathPoints.Length)
                return;

            var target = Model.PathPoints[_currentCornerIndex];
            var direction = (target - transform.position).normalized;

            var offset = Model.PositionVelocity * Time.deltaTime;
            var distance = Vector3.Distance(transform.position, target);

            if (offset >= distance)
            {
                transform.position = target;
                _currentCornerIndex++;

                if (_currentCornerIndex >= Model.PathPoints.Length)
                {
                    _moving = false;
                    _currentCornerIndex = 0;
                }
            }
            else
                transform.position += direction * offset;
        }

        public void Setup(UnitController unit)
        {
            if (_playerProvider.Player.MoveCount > 0)
            {
                _unit = unit;
                View.ViewMove(transform.position, Model.Speed);
            }
        }

        public void Clear() => View.ClearMove();

        [ServerRpc(RequireOwnership = false)]
        public void SetDestinationServerRpc(Vector3 point, ServerRpcParams rpcParams = default)
        {
            var sender = rpcParams.Receive.SenderClientId;

            if (!_playersContainer.TryGet(sender, out var player) || player.MoveCount <= 0)
                return;

            Model.PathPoints = Array.Empty<Vector3>();

            if (Vector3.Distance(transform.position, point) <= Model.Speed)
            {
                var path = new NavMeshPath();
                if (NavMesh.CalculatePath(transform.position, point, NavMesh.AllAreas, path))
                    Model.PathPoints = path.corners;
            }

            SetDestinationClientRpc(Model.PathPoints, new ClientRpcParams().For(sender));
        }

        [ClientRpc]
        public void SetDestinationClientRpc(Vector3[] pathPoints, ClientRpcParams _)
        {
            Model.PathPoints = pathPoints;
            if (_selector.Selected == _unit)
                View.OnDestinationChanged(Helper.IsDestinationValid, Model.PathPoints);
            Attack.CalculateAttack(Helper.IsDestinationValid ? Model.PathPoints[^1] : transform.position);
        }

        [ServerRpc(RequireOwnership = false)]
        public void MoveDestinationServerRpc(ServerRpcParams rpcParams = default)
        {
            var sender = rpcParams.Receive.SenderClientId;
            if (!_playersContainer.TryGet(sender, out var player) || player.MoveCount <= 0)
                return;

            if (Helper.IsDestinationValid)
            {
                _moving = true;
                player.MoveCount -= 1;
                OnMoveDestinationClientRpc(new ClientRpcParams().For(sender));
            }
        }

        [ClientRpc]
        public void OnMoveDestinationClientRpc(ClientRpcParams _) => _selector.UnSelect(_unit);

        [ServerRpc(RequireOwnership = false)]
        public void ClearDestinationServerRpc(ServerRpcParams rpcParams = default)
        {
            Model.PathPoints = Array.Empty<Vector3>();
            SetDestinationClientRpc(Model.PathPoints, new ClientRpcParams().For(rpcParams.Receive.SenderClientId));
        }
    }
}