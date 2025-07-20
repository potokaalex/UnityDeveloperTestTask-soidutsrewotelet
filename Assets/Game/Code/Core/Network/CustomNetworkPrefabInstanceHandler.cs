using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

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
            var container = ProjectContext.Instance.Container.Resolve<SceneContextRegistry>().GetContainerForScene(SceneManager.GetActiveScene());
            var instance = _instantiator.InstantiatePrefabForComponent<NetworkObject>(container, _gameObject);
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            return instance;
        }

        public void Destroy(NetworkObject networkObject) => Object.Destroy(networkObject.gameObject);
    }
}