using Unity.Netcode;

namespace Game.Code.Gameplay.Unit
{
    public struct UnitControllerPayload : INetworkSerializable
    {
        public TeamType Team;
        public int Id;
        
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Team);
            serializer.SerializeValue(ref Id);
        }
    }
}