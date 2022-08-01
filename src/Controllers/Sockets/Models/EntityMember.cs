using HttpDriver.Controllers.Sockets.Packets;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets.Models
{
    public class EntityMember
    {
        private readonly CreateEntityMember _packet;
        private readonly EntityMemberTracker _tracker;
        private DateTime _nextTime;

        #region Constructors

        public EntityMember(CreateEntity entityPacket, CreateEntityMember memberPacket, IMemoryService service)
        {
            _packet = memberPacket;
            _tracker = new EntityMemberTracker(entityPacket.Address + memberPacket.Offset, memberPacket.Size, service);
        }

        #endregion

        #region Methods

        public void Receive(ChangeEntityMember change)
        {
            _tracker.TryWrite(change.Buffer);
        }

        public UpdateEntityMember? Update()
        {
            if (DateTime.Now < _nextTime) return null;
            SetNextTime();
            if (!_tracker.TryRead(out var buffer) || buffer == null) return null;
            return new UpdateEntityMember { Offset = _packet.Offset, Buffer = buffer };
        }

        private void SetNextTime()
        {
            if (_packet.Interval == 0) return;
            var currentTicks = DateTime.Now.Ticks;
            var intervalTicks = TimeSpan.TicksPerMillisecond * _packet.Interval;
            _nextTime = new DateTime(currentTicks - currentTicks % intervalTicks + intervalTicks);
        }

        #endregion
    }
}