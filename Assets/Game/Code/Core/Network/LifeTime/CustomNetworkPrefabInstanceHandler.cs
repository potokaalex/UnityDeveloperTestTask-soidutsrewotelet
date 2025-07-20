using Unity.Netcode;
using UnityEngine;
using Zenject;

namespace Game.Code.Core.Network.LifeTime
{
    public class CustomNetworkPrefabInstanceHandler : INetworkPrefabInstanceHandler
    {
        private readonly GameObject _gameObject;
        private readonly IInstantiator _instantiator;

        public CustomNetworkPrefabInstanceHandler(GameObject gameObject, IInstantiator instantiator)
        {
            _gameObject = gameObject;
            _instantiator = instantiator;
        }

        public NetworkObject Instantiate(ulong ownerClientId, Vector3 position, Quaternion rotation)
        {
            var instance = _instantiator.InstantiatePrefabForComponent<NetworkObject>(_gameObject, position, rotation, null);
            if (NetworkManager.Singleton.IsServer)
                instance.ChangeOwnership(ownerClientId);
            return instance;
        }

        public void Destroy(NetworkObject networkObject) => networkObject.Despawn();
    }
}