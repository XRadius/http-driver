using HttpDriver.Controllers.Sockets.Abstracts.Interfaces;
using HttpDriver.Controllers.Sockets.Packets;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets.Models
{
    public class Entity : IEntityProvider
    {
        private readonly EntityCreate _entity;
        private readonly Dictionary<uint, EntityMember> _members;

        #region Constructors

        public Entity(EntityCreate entity, IMemoryService service)
        {
            _entity = entity;
            _members = entity.Members.ToDictionary(x => x.Offset, x => new EntityMember(entity, x, service));
        }

        #endregion

        #region Methods

        private EntityUpdateEntity? Process()
        {
            var members = _members.Values
                .Select(x => x.Update())
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();
            return members.Count != 0
                ? new EntityUpdateEntity { Id = _entity.Id, Members = members }
                : null;
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
            return Process();
        }

        #endregion
    }
}