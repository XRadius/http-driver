namespace HttpDriver.Controllers.Sockets.Abstracts.Enums
{
    public enum PacketType : byte
    {
        Activity,
        ChangeEntity,
        CreateEntity,
        DeleteEntity,
        Update
    }
}