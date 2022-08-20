using HttpDriver.Controllers.Sockets.Extensions;
using HttpDriver.Controllers.Sockets.Packets;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets.Models
{
    public class EntityMember
    {
        private readonly ulong _address;
        private readonly EntityCreateMember _member;
        private readonly byte[] _previous;
        private readonly IMemoryService _service;
        private DateTime _nextTime;

        #region Constructors

        public EntityMember(EntityCreate entity, EntityCreateMember member, IMemoryService service)
        {
            _address = entity.Address + member.Offset;
            _member = member;
            _previous = new byte[member.Size];
            _service = service;
        }

        #endregion

        #region Methods

        public void Receive(EntityChangeMember change)
        {
            if (change.Deltas.Count == 0)
            {
                var buffer = change.Buffer;
                if (buffer.Length != _member.Size || !_service.Write(_address, buffer)) return;
                Buffer.BlockCopy(buffer, 0, _previous, 0, buffer.Length);
            }
            else
            {
                var buffer = new byte[_member.Size];
                if (!_service.Read(_address, buffer)) return;
                Transform(change, buffer);
                _service.Write(_address, buffer);
            }
        }

        public EntityUpdateEntityMember? Update()
        {
            if (DateTime.Now < _nextTime) return null;
            SetNextTime();
            return CheckUpdate();
        }

        private EntityUpdateEntityMember? CheckUpdate()
        {
            var buffer = new byte[_member.Size];
            if (!_service.Read(_address, buffer) || _previous.SequenceEqual(buffer)) return null;
            Buffer.BlockCopy(buffer, 0, _previous, 0, buffer.Length);
            return new EntityUpdateEntityMember { Offset = _member.Offset, Buffer = buffer };
        }

        private void SetNextTime()
        {
            if (_member.Interval == 0) return;
            var currentTicks = DateTime.Now.Ticks;
            var intervalTicks = TimeSpan.TicksPerMillisecond * _member.Interval;
            _nextTime = new DateTime(currentTicks - currentTicks % intervalTicks + intervalTicks);
        }

        #endregion

        #region Statics

        private static void Transform(EntityChangeMember change, byte[] buffer)
        {
            foreach (var delta in change.Deltas)
            {
                var count = delta.Type.Size();
                if (delta.Offset + count > buffer.Length) continue;
                var current = new byte[count];
                Buffer.BlockCopy(buffer, (int)delta.Offset, current, 0, count);
                var result = delta.Type.Transform(current, delta.Buffer);
                Buffer.BlockCopy(result, 0, buffer, (int)delta.Offset, count);
            }
        }

        #endregion
    }
}