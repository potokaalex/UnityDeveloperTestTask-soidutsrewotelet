using Unity.Netcode;
using UnityEngine;

namespace Game.Code.Core.Network
{
    public class CustomNetworkPrefabInstanceHandler : INetworkPrefabInstanceHandler
    {
        private readonly GameObject _gameObject;
        private readonly Instantiator _instantiator;

        public CustomNetworkPrefabInstanceHandler(GameObject gameObject, Instantiator instantiator)
        {
            _gameObject = gameObject;
            _instantiator = instantiator;
        }

        public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
        {
            var instance = _instantiator.InstantiatePrefabForComponent<NetworkObject>(_gameObject);
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance;
        }

        public void Destroy(NetworkObject networkObject) => Object.Destroy(networkObject.gameObject);
    }
}