using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Code.Gameplay.Obstacle
{
    public class ObstaclesSpawner : NetworkBehaviour
    {
        public List<ObstacleController> Prefabs;
        public ObstacleSpawnPreset[] Presets;
        public float AreaSize;
        private readonly List<ObstacleController> _obstacles = new();
        
        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                foreach (var preset in Presets)
                    CreateObstacles(preset);
                NetworkManager.OnClientConnectedCallback += OnClientConnected;
            }
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
                NetworkManager.OnClientConnectedCallback -= OnClientConnected;
        }

        private void OnClientConnected(ulong clientId)
        {
            foreach (var obstacle in _obstacles)
            {
                SendObstacleToClientRpc(obstacle.Type, obstacle.transform.position, new ClientRpcParams
                {
                    Send = new ClientRpcSendParams { TargetClientIds = new[] { clientId } }
                });
            }
        }

        [ClientRpc]
        private void SendObstacleToClientRpc(int type, Vector3 position, ClientRpcParams rpcParams) => CreateObstacle(type, position);

        private void CreateObstacles(ObstacleSpawnPreset preset)
        {
            for (var i = 0; i < Random.Range(preset.MinCount, preset.MaxCount + 1); i++)
            {
                var halfSize = AreaSize / 2f;
                var position = transform.position + new Vector3(Random.Range(-halfSize, halfSize), 0, Random.Range(-halfSize, halfSize));
                _obstacles.Add(CreateObstacle(preset.Type, position));
            }
        }

        private ObstacleController CreateObstacle(int type, Vector3 position)
        {
            var prefab = Prefabs.First(x => x.Type == type);
            var instance = Instantiate(prefab, transform);
            instance.transform.position = position;
            return instance;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(transform.position, new Vector3(AreaSize, 0, AreaSize));
            Gizmos.color = Color.white;
        }
    }
}