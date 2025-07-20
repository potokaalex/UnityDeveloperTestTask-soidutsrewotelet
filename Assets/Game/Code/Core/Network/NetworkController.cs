using Unity.Netcode;

namespace Game.Code.Core.Network
{
    public class NetworkController : INetworkController
    {
        public bool IsServer => NetworkManager.Singleton.IsServer;
    }
}