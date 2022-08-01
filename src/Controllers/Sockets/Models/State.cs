using HttpDriver.Controllers.Sockets.Abstracts.Enums;
using HttpDriver.Controllers.Sockets.Packets;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets.Models
{
    public class State
    {
        private readonly Dictionary<ulong, Entity> _entities;
        private readonly IMemoryService _service;
        private DateTime _inactiveTime;

        #region Constructors

        public State(IMemoryService service)
        {
            _entities = new Dictionary<ulong, Entity>();
            _service = service;
            SetInactiveTime();
        }

        #endregion

        #region Methods

        public void Receive(BinaryReader stream)
        {
            while (stream.BaseStream.Position < stream.BaseStream.Length)
            {
                switch ((PacketType)stream.ReadByte())
                {
                    case PacketType.Activity:
                        SetInactiveTime();
                        break;
                    case PacketType.ChangeEntity:
                        var change = ChangeEntity.Create(stream);
                        if (!_entities.TryGetValue(change.Address, out var entity)) break;
                        foreach (var child in change.Changes) entity.Receive(child);
                        break;
                    case PacketType.CreateEntity:
                        var create = CreateEntity.Create(stream);
                        if (_entities.ContainsKey(create.Address)) break;
                        _entities[create.Address] = new Entity(create, _service);
                        break;
                    case PacketType.DeleteEntity:
                        var delete = DeleteEntity.Create(stream);
                        _entities.Remove(delete.Address);
                        break;
                }
            }
        }

        public UpdateArray? Update()
        {
            var entities = _entities.Values
                .Select(x => x.Update())
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();
            return entities.Count != 0
                ? new UpdateArray { Entities = entities }
                : null;
        }

        private void SetInactiveTime()
        {
            _inactiveTime = DateTime.Now.AddSeconds(30);
        }

        #endregion

        #region Properties

        public bool IsActive => _inactiveTime >= DateTime.Now;

        #endregion
    }
}