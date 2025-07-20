using System.Linq;
using Game.Code.Core;
using Game.Code.Core.Network;
using Game.Code.Core.Network.LifeTime;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay.Player
{
    public class PlayersSpawner : MonoBehaviour, IOnClientConnectedReceiver
    {
        public PlayerController PlayerPrefab;
        private int _lastId;
        private PlayersContainer _container;
        private Instantiator _instantiator;
        private INetworkController _networkController;

        [Inject]
        public void Construct(PlayersContainer container, Instantiator instantiator, INetworkController networkController)
        {
            _networkController = networkController;
            _instantiator = instantiator;
            _container = container;
        }

        public void OnClientConnected(ulong clientId)
        {
            if (_networkController.IsServer)
            {
                var instance = _instantiator.InstantiatePrefabForComponent<PlayerController>(PlayerPrefab.gameObject);
                instance.Initialize(clientId, GetTeam());
                instance.NetworkObject.SpawnWithOwnership(clientId);
            }
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