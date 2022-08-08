using HttpDriver.Controllers.Sockets.Abstracts.Interfaces;
using HttpDriver.Controllers.Sockets.Packets;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets.Models
{
    public class BatchEntity : IEntityProvider
    {
        private readonly byte[] _buffer;
        private readonly EntityCreate _entity;
        private readonly uint _interval;
        private readonly Dictionary<uint, BatchEntityMember> _members;
        private readonly IMemoryService _service;
        private DateTime _nextTime;

        #region Constructors

        public BatchEntity(EntityCreate entity, IMemoryService service)
        {
            _buffer = new byte[entity.Members.Max(x => x.Offset + x.Size)];
            _entity = entity;
            _interval = entity.Members.Min(x => x.Interval);
            _members = entity.Members.ToDictionary(x => x.Offset, x => new BatchEntityMember(entity, x, service));
            _service = service;
        }

        #endregion

        #region Methods

        private EntityUpdateEntity? Process()
        {
            var members = _members.Values
                .Select(x => x.Update(_buffer))
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();
            return members.Count != 0
                ? new EntityUpdateEntity { Id = _entity.Id, Members = members }
                : null;
        }

        private void SetNextTime()
        {
            if (_interval == 0) return;
            var currentTicks = DateTime.Now.Ticks;
            var intervalTicks = TimeSpan.TicksPerMillisecond * _interval;
            _nextTime = new DateTime(currentTicks - currentTicks % intervalTicks + intervalTicks);
        }

        #endregion

        #region Implementation of IEntityProvider

        public void Receive(EntityChangeMember change)
        {
            if (!_members.TryGetValue(change.Offset, out var member)) return;
            member.Receive(change);
        }

        public EntityUpdateEntity? Update()
        {
            if (DateTime.Now < _nextTime) return null;
            SetNextTime();
            if (!_service.Read(_entity.Address, _buffer)) return null;
            return Process();
        }

        #endregion
    }
}