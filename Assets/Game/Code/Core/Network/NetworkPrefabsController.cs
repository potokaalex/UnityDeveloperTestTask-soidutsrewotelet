using System;
using Unity.Netcode;

namespace Game.Code.Core.Network
{
    public class NetworkPrefabsController : IDisposable
    {
        public NetworkPrefabsController(Instantiator instantiator)
        {
            //Register here, because spawn events can arrive before zen.Initialize
            foreach (var prefab in NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs)
                NetworkManager.Singleton.PrefabHandler.AddHandler(prefab.Prefab,
                    new CustomNetworkPrefabInstanceHandler(prefab.Prefab, instantiator));
        }

        public void Dispose()
        {
            if (NetworkManager.Singleton)
                foreach (var prefab in NetworkManager.Singleton.NetworkConfig.Prefabs.Prefabs)
                    NetworkManager.Singleton.PrefabHandler.RemoveHandler(prefab.Prefab);
        }
    }
}