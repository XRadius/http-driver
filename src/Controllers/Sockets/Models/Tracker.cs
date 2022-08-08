using HttpDriver.Controllers.Sockets.Abstracts.Interfaces;
using HttpDriver.Controllers.Sockets.Packets;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets.Models
{
    public class Tracker
    {
        private readonly Dictionary<uint, IEntityProvider> _entities;
        private readonly IMemoryService _service;

        #region Constructors

        public Tracker(IMemoryService service)
        {
            _entities = new Dictionary<uint, IEntityProvider>();
            _service = service;
        }

        #endregion

        #region Methods

        public void Receive(object packet)
        {
            switch (packet)
            {
                case EntityChange change:
                    if (!_entities.TryGetValue(change.Id, out var entity)) break;
                    foreach (var child in change.Changes) entity.Receive(child);
                    break;
                case EntityCreate create:
                    if (_entities.ContainsKey(create.Id)) break;
                    _entities[create.Id] = create.RequestBatch ? new BatchEntity(create, _service) : new Entity(create, _service);
                    break;
                case EntityDelete delete:
                    _entities.Remove(delete.Id);
                    break;
            }
        }

        public void Update(BinaryWriter stream)
        {
            var entities = _entities.Values
                .Select(x => x.Update())
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();
            var update = entities.Count != 0
                ? new EntityUpdate { Entities = entities }
                : null;
            update?.Write(stream);
        }

        #endregion
    }
}