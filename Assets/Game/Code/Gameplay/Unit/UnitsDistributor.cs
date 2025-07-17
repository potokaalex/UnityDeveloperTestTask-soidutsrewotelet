using Unity.Netcode;
using Zenject;

namespace Game.Code.Gameplay.Unit
{
    public class UnitsDistributor : NetworkBehaviour
    {
        private UnitsContainer _container;
        private int _clientsCount;

        [Inject]
        public void Construct(UnitsContainer container)
        {
            _container = container;
        }
        
        public override void OnNetworkSpawn() => NetworkManager.OnClientConnectedCallback += OnClientConnected;

        public override void OnNetworkDespawn() => NetworkManager.OnClientConnectedCallback -= OnClientConnected;

        private void OnClientConnected(ulong clientId)
        {
            using var d = UnityEngine.Pool.ListPool<UnitController>.Get(out var units);

            if (_clientsCount == 0)
                _container.Get(TeamType.A, units);
            else if(_clientsCount == 1)
                _container.Get(TeamType.B, units);
            
            foreach (var unit in units)
                unit.NetworkObject.ChangeOwnership(clientId);

            _clientsCount++;
        }
    }
}