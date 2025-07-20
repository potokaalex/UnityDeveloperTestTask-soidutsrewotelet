using Game.Code.Core.Network;
using Game.Code.Gameplay.Obstacle;
using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Game.Code.Gameplay
{
    public class GroundController : NetworkBehaviour, IInitializable, IOnClientConnectedReceiver
    {
        public ObstaclesSpawner ObstaclesSpawner;
        private INetworkController _networkController;
        private int _seed;

        [Inject]
        public void Construct(INetworkController networkController) => _networkController = networkController;

        public void Initialize()
        {
            if (_networkController.IsServer)
            {
                _seed = Random.Range(int.MinValue, int.MaxValue);
                CreateGround(_seed);
            }
        }

        public void OnClientConnected(ulong clientId)
        {
            if (_networkController.IsServer)
                CreateGroundClientRpc(_seed, new ClientRpcParams().For(clientId));
        }

        [ClientRpc]
        public void CreateGroundClientRpc(int seed, ClientRpcParams _) => CreateGround(seed);

        private void CreateGround(int seed) => ObstaclesSpawner.Spawn(seed);
    }
}