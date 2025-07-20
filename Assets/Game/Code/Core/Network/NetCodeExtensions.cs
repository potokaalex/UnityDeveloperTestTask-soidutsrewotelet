using Unity.Netcode;

namespace Game.Code.Core.Network
{
    public static class NetCodeExtensions
    {
        public static ClientRpcParams For(this ClientRpcParams clientRpcParams, ulong clientId)
        {
            clientRpcParams.Send = new ClientRpcSendParams { TargetClientIds = new[] { clientId } };
            return clientRpcParams;
        }

        public static void Initialize<T>(this NetworkVariable<T> variable, NetworkBehaviour behaviour, T defaultValue)
        {
            variable.Initialize(behaviour);
            variable.Value = defaultValue;
        }
    }
}