using System.Linq;
using Unity.Netcode;
using Zenject;

namespace Game.Code.Gameplay.Player
{
    public class PlayerSpawner : NetworkBehaviour
    {
        public PlayerController PlayerPrefab;
        private int _lastId;
        private PlayersContainer _container;

        [Inject]
        public void Construct(PlayersContainer container) => _container = container;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                NetworkManager.OnClientConnectedCallback += OnClientConnected;
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
                NetworkManager.OnClientConnectedCallback -= OnClientConnected;
        }

        private void OnClientConnected(ulong clientId)
        {
            var instance = Instantiate(PlayerPrefab);

            instance.Id.Initialize(instance);
            instance.Id.Value = _lastId++;

            instance.Team.Initialize(instance);
            instance.Team.Value = GetTeam();

            instance.NetworkObject.Spawn();
            instance.NetworkObject.ChangeOwnership(clientId);
        }

        private TeamType GetTeam()
        {
            using var d = UnityEngine.Pool.ListPool<PlayerController>.Get(out var players);
            _container.Get(players);
            if (players.All(x => x.Team.Value != TeamType.A))
                return TeamType.A;

            if (players.All(x => x.Team.Value != TeamType.B))
                return TeamType.B;

            return TeamType.None;
        }
    }
}