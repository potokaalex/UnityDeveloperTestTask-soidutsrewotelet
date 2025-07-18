using Unity.Netcode;

namespace Game.Code.Gameplay.Player
{
    public class PlayerController : NetworkBehaviour
    {
        public TeamType Team { get; private set; }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
                NetworkManager.OnClientConnectedCallback += OnClientConnected;
        }

        public override void OnNetworkDespawn()
        {
            if (IsServer)
                NetworkManager.OnClientConnectedCallback -= OnClientConnected;
        }

        private void OnClientConnected(ulong clientId)
        {
            var team = TeamType.None;

            if (clientId == NetworkManager.ConnectedClientsIds[0])
                team = TeamType.A;
            else if (clientId == NetworkManager.ConnectedClientsIds[1])
                team = TeamType.B;

            SetDataForClientRpc(team, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = new[] { clientId } } });
        }

        [ClientRpc]
        private void SetDataForClientRpc(TeamType team, ClientRpcParams rpcParams = default) => Team = team;
    }
}