using Game.Code.Core.Network;
using Game.Code.Core.Network.LifeTime;
using Game.Code.Gameplay.Obstacle;
using Unity.Netcode;
using UnityEngine;

namespace Game.Code.Gameplay
{
    public class GroundController : NetworkBehaviour, IOnClientConnectedReceiver
    {
        public ObstaclesSpawner ObstaclesSpawner;
        private int _seed;

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                _seed = Random.Range(int.MinValue, int.MaxValue);
                CreateGround(_seed);
            }
        }

        public void OnClientConnected(ulong clientId)
        {
            if (IsServer)
                CreateGroundClientRpc(_seed, new ClientRpcParams().For(clientId));
        }

        [ClientRpc]
        public void CreateGroundClientRpc(int seed, ClientRpcParams _) => CreateGround(seed);

        private void CreateGround(int seed) => ObstaclesSpawner.Spawn(seed);
    }
}