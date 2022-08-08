using HttpDriver.Controllers.Sockets.Packets;

namespace HttpDriver.Controllers.Sockets.Abstracts.Interfaces
{
    public interface IEntityProvider
    {
        #region Methods

        void Receive(EntityChangeMember change);

        EntityUpdateEntity? Update();

        #endregion
    }
}