using System;
using System.Collections.Generic;
using Unity.Netcode;
using Zenject;

namespace Game.Code.Core.Network.LifeTime
{
    public class NetworkLifetimeController : IInitializable, IDisposable
    {
        private readonly DiContainer _container;

        public NetworkLifetimeController(DiContainer container) => _container = container;

        public void Initialize() => NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;

        public void Dispose()
        {
            if (NetworkManager.Singleton)
                NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }

        private void OnClientConnected(ulong clientId)
        {
            foreach (var receiver in _container.Resolve<List<IOnClientConnectedReceiver>>())
                receiver.OnClientConnected(clientId);
        }
    }
}