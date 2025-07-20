using System.Linq;
using Game.Code.Core;
using Game.Code.Gameplay.Unit;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay.Player
{
    public class PlayersSpawner : MonoBehaviour
    {
        public PlayerController PlayerPrefab;
        private int _lastId;
        private PlayersContainer _container;
        private Instantiator _instantiator;
        private UnitsContainer _unitsContainer;

        [Inject]
        public void Construct(PlayersContainer container, Instantiator instantiator, UnitsContainer unitsContainer)
        {
            _unitsContainer = unitsContainer;
            _instantiator = instantiator;
            _container = container;
        }

        public void Spawn(ulong clientId)
        {
            var instance = _instantiator.InstantiatePrefabForComponent<PlayerController>(PlayerPrefab.gameObject);
            instance.Initialize(clientId, GetTeam());
            instance.NetworkObject.SpawnWithOwnership(clientId);

            using var d = UnityEngine.Pool.ListPool<UnitController>.Get(out var units);
            _unitsContainer.Get(units);
            foreach (var unit in units)
                unit.NetworkObject.ChangeOwnership(clientId);
        }

        public void Despawn(ulong clientId, ulong serverClientId)
        {
            using var d = UnityEngine.Pool.ListPool<UnitController>.Get(out var units);
            _unitsContainer.Get(units);
            foreach (var unit in units)
                if (unit.OwnerClientId == clientId)
                    unit.NetworkObject.ChangeOwnership(serverClientId);
        }

        private TeamType GetTeam()
        {
            using var d = UnityEngine.Pool.ListPool<PlayerController>.Get(out var players);
            _container.Get(players);
            if (players.All(x => x.Team != TeamType.A))
                return TeamType.A;

            if (players.All(x => x.Team != TeamType.B))
                return TeamType.B;

            return TeamType.None;
        }
    }
}