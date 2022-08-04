using HttpDriver.Controllers.Sockets.Packets;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets.Models
{
    public class EntityMember
    {
        private readonly EntityCreateMember _member;
        private readonly EntityMemberTracker _tracker;
        private DateTime _nextTime;

        #region Constructors

        public EntityMember(EntityCreate entity, EntityCreateMember member, IMemoryService service)
        {
            _member = member;
            _tracker = new EntityMemberTracker(entity.Address + member.Offset, member.Size, service);
        }

        #endregion

        #region Methods

        public void Receive(EntityChangeMember change)
        {
            _tracker.TryWrite(change.Buffer);
        }

        public EntityUpdateEntityMember? Update()
        {
            if (DateTime.Now < _nextTime) return null;
            SetNextTime();
            if (!_tracker.TryRead(out var buffer) || buffer == null) return null;
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
    }
}