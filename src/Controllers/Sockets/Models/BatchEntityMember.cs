using HttpDriver.Controllers.Sockets.Packets;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets.Models
{
    public class BatchEntityMember
    {
        private readonly ulong _address;
        private readonly uint _offset;
        private readonly byte[] _previousBuffer;
        private readonly IMemoryService _service;

        #region Constructors

        public BatchEntityMember(EntityCreate entity, EntityCreateMember member, IMemoryService service)
        {
            _address = entity.Address + member.Offset;
            _offset = member.Offset;
            _previousBuffer = new byte[member.Size];
            _service = service;
        }

        #endregion

        #region Methods

        public void Receive(EntityChangeMember change)
        {
            var buffer = change.Buffer;
            if (buffer.Length != _previousBuffer.Length || !_service.Write(_address, buffer)) return;
            Buffer.BlockCopy(buffer, 0, _previousBuffer, 0, buffer.Length);
        }

        public EntityUpdateEntityMember? Update(byte[] entity)
        {
            var buffer = new byte[_previousBuffer.Length];
            Buffer.BlockCopy(entity, (int)_offset, buffer, 0, buffer.Length);
            if (buffer.SequenceEqual(_previousBuffer)) return null;
            Buffer.BlockCopy(buffer, 0, _previousBuffer, 0, buffer.Length);
            return new EntityUpdateEntityMember { Offset = _offset, Buffer = buffer };
        }

        #endregion
    }
}