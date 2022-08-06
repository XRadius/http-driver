using System.Text.Json.Serialization;
using HttpDriver.Controllers.Sockets.Abstracts.Enums;
using HttpDriver.Controllers.Sockets.Abstracts.Interfaces;

namespace HttpDriver.Controllers.Sockets.Packets
{
    public class BasicSync : IPacketWriter
    {
        #region Constructors

        public static BasicSync Create(BinaryReader stream)
        {
            var id = stream.ReadByte();
            return new BasicSync { Id = id };
        }

        #endregion

        #region Properties

        [JsonPropertyName("id")]
        public byte Id { get; init; }

        #endregion

        #region Implementation of IPacketWriter

        public void Write(BinaryWriter stream)
        {
            stream.Write((byte)PacketType.BasicSync);
            stream.Write(Id);
        }

        #endregion
    }
}