namespace HttpDriver.Controllers.Sockets.Abstracts.Enums
{
    public enum PacketType : byte
    {
        BasicAlive,
        BasicSync,
        EntityChange,
        EntityCreate,
        EntityDelete,
        EntityUpdate
    }
}