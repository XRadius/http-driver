using HttpDriver.Controllers.Sockets.Packets;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets.Models
{
    public class Entity
    {
        private readonly EntityCreate _entity;
        private readonly Dictionary<ushort, EntityMember> _members;

        #region Constructors

        public Entity(EntityCreate entity, IMemoryService service)
        {
            _entity = entity;
            _members = entity.Members.ToDictionary(x => x.Offset, x => new EntityMember(entity, x, service));
        }

        #endregion

        #region Methods

        public void Receive(EntityChangeMember change)
        {
            if (!_members.TryGetValue(change.Offset, out var member)) return;
            member.Receive(change);
        }

        public EntityUpdateEntity? Update()
        {
            var members = _members.Values
                .Select(x => x.Update())
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();
            return members.Count != 0
                ? new EntityUpdateEntity { Address = _entity.Address, Members = members }
                : null;
        }

        #endregion
    }
}