using System;
using Game.Code.Core.Network.LifeTime;
using Unity.Netcode;
using Zenject;

namespace Game.Code.Core.Network
{
    public class NetworkPrefabsController : IInitializable, IDisposable
    {
        private readonly IInstantiator _instantiator;

        public NetworkPrefabsController(IInstantiator instantiator) => _instantiator = instantiator;

        public void Initialize()
        {
            foreach (var prefab in NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs)
                NetworkManager.Singleton.PrefabHandler.AddHandler(prefab.Prefab,
                    new CustomNetworkPrefabInstanceHandler(prefab.Prefab, _instantiator));
        }

        public void Dispose()
        {
            if (NetworkManager.Singleton)
                foreach (var prefab in NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs)
                    NetworkManager.Singleton.PrefabHandler.RemoveHandler(prefab.Prefab);
        }
    }
}