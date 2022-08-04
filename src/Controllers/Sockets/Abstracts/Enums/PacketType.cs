namespace HttpDriver.Controllers.Sockets.Abstracts.Enums
{
    public enum PacketType : byte
    {
        Activity,
        EntityChange,
        EntityCreate,
        EntityDelete,
        EntityUpdate
    }
}