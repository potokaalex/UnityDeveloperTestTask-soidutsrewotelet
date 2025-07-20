using System;
using System.Collections.Generic;
using System.Linq;
using Game.Code.Core.Network;
using Game.Code.Gameplay.Player;
using Game.Code.Gameplay.Unit.View;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Pool;
using Zenject;

namespace Game.Code.Gameplay.Unit
{
    public class UnitAttackController : NetworkBehaviour
    {
        public UnitHelper Helper;
        public UnitView View;
        public UnitModel Model;
        private PlayersContainer _playersContainer;
        private UnitsContainer _container;
        private UnitsSelector _selector;
        private IPlayerProvider _playerProvider;
        private UnitController _unit;

        private float FullAttackRadius => Model.AttackRadius + Model.BodyRadius;

        [Inject]
        public void Construct(PlayersContainer playersContainer, UnitsContainer container, UnitsSelector selector, IPlayerProvider playerProvider)
        {
            _playersContainer = playersContainer;
            _container = container;
            _selector = selector;
            _playerProvider = playerProvider;
        }

        public void Setup(UnitController unit)
        {
            _unit = unit;
            CalculateAttack(transform.position);
        }

        public void Clear()
        {
            using var d = GetAllEnemies(out var list);
            View.ClearAttack(list);
        }

        public void Attack(UnitController unit)
        {
            if (unit != _unit)
                AttackServerRpc(unit.Id);
        }

        [ServerRpc]
        public void AttackServerRpc(ulong unitId, ServerRpcParams rpcParams = default)
        {
            var sender = rpcParams.Receive.SenderClientId;

            if (!_playersContainer.TryGet(sender, out var player) || player.AttackCount <= 0)
                return;

            using var d = GetUnitsForAttack(transform.position, player.Team, out var units);
            if (units.Any(x => x.Id == unitId))
            {
                _container.Get(unitId).NetworkObject.Despawn();
                player.AttackCount -= 1;
                AttackClientRpc(new ClientRpcParams().For(sender));
            }
        }

        [ClientRpc]
        private void AttackClientRpc(ClientRpcParams _) => _selector.UnSelect(_unit);

        public void CalculateAttack(Vector3 point)
        {
            if (_selector.Selected == _unit && _playerProvider.Player.AttackCount > 0)
            {
                using var d = GetUnitsForAttack(point, _playerProvider.Player.Team, out var forAttack);
                using var d1 = GetAllEnemies(out var enemies);
                View.ViewAttack(point, FullAttackRadius, forAttack, enemies);
            }
        }

        private PooledObject<List<UnitController>> GetUnitsForAttack(Vector3 point, TeamType playerTeam, out List<UnitController> outList)
        {
            using var d = UnityEngine.Pool.ListPool<UnitController>.Get(out var units);
            _container.Get(units);

            var result = UnityEngine.Pool.ListPool<UnitController>.Get(out outList);
            foreach (var unit in units)
                if (Helper.AreEnemies(playerTeam, unit.Team) && unit.IsInRange(point, FullAttackRadius))
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
    }
}