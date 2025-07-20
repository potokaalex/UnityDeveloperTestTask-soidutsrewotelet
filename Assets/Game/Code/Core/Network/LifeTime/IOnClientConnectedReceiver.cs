namespace Game.Code.Core.Network.LifeTime
{
    public interface IOnClientConnectedReceiver
    {
        public void OnClientConnected(ulong clientId);
    }
}