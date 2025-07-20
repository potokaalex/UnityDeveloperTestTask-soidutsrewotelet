using Game.Code.Core.Network;
using Game.Code.Gameplay.Obstacle;
using Game.Code.Gameplay.Player;
using Game.Code.Gameplay.Unit;
using Unity.Netcode;
using UnityEngine;

namespace Game.Code.Gameplay
{
    public class MapController : NetworkBehaviour
    {
        public ObstaclesSpawner ObstaclesSpawner;
        public UnitsSpawner UnitsSpawner;
        public PlayersSpawner PlayersSpawner;
        public MatchController MatchController;
        private int _seed;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                _seed = Random.Range(int.MinValue, int.MaxValue);
                CreateGround(_seed);
                UnitsSpawner.Spawn();
                NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisConnected;
            }

            MatchController.Initialize();
        }

        public override void OnNetworkDespawn()
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= OnClientDisConnected;
        }

        public void OnClientConnected(ulong clientId)
        {
            CreateGroundClientRpc(_seed, new ClientRpcParams().For(clientId));
            PlayersSpawner.Spawn(clientId);
        }

        public void OnClientDisConnected(ulong clientId) => PlayersSpawner.Despawn(clientId, OwnerClientId);

        [ClientRpc]
        public void CreateGroundClientRpc(int seed, ClientRpcParams _) => CreateGround(seed);

        private void CreateGround(int seed) => ObstaclesSpawner.Spawn(seed);
    }
}