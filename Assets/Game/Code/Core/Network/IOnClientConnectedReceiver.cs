namespace Game.Code.Core.Network
{
    public interface IOnClientConnectedReceiver
    {
        public void OnClientConnected(ulong clientId);
    }
}