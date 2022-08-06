using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets.Models
{
    public class EntityMemberTracker
    {
        private readonly ulong _address;
        private readonly byte[] _previousBuffer;
        private readonly IMemoryService _service;

        #region Constructors

        public EntityMemberTracker(ulong address, uint bufferSize, IMemoryService service)
        {
            _address = address;
            _previousBuffer = new byte[bufferSize];
            _service = service;
        }

        #endregion

        #region Methods

        public bool TryRead(out byte[]? result)
        {
            var buffer = new byte[_previousBuffer.Length];

            if (!_service.Read(_address, buffer))
            {
                result = null;
                return false;
            }

            if (_previousBuffer.SequenceEqual(buffer))
            {
                result = null;
                return false;
            }

            Buffer.BlockCopy(buffer, 0, _previousBuffer, 0, buffer.Length);
            result = buffer;
            return true;
        }

        public bool TryWrite(byte[] buffer)
        {
            if (buffer.Length != _previousBuffer.Length || !_service.Write(_address, buffer)) return false;
            Buffer.BlockCopy(buffer, 0, _previousBuffer, 0, buffer.Length);
            return true;
        }

        #endregion
    }
}