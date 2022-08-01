using HttpDriver.Controllers.Sockets.Packets;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets.Models
{
    public class Entity
    {
        private readonly Dictionary<ushort, EntityMember> _members;
        private readonly CreateEntity _packet;

        #region Constructors

        public Entity(CreateEntity packet, IMemoryService service)
        {
            _members = packet.Members.ToDictionary(x => x.Offset, x => new EntityMember(packet, x, service));
            _packet = packet;
        }

        #endregion

        #region Methods

        public void Receive(ChangeEntityMember change)
        {
            if (!_members.TryGetValue(change.Offset, out var member)) return;
            member.Receive(change);
        }

        public UpdateEntity? Update()
        {
            var members = _members.Values
                .Select(x => x.Update())
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();
            return members.Count != 0
                ? new UpdateEntity { Address = _packet.Address, Members = members }
                : null;
        }

        #endregion
    }
}