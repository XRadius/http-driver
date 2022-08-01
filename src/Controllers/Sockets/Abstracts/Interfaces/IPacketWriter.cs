namespace HttpDriver.Controllers.Sockets.Abstracts.Interfaces
{
    public interface IPacketWriter
    {
        #region Methods

        void Write(BinaryWriter stream);

        #endregion
    }
}