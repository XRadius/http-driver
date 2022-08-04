using HttpDriver.Controllers.Sockets.Packets;
using HttpDriver.Utilities;

namespace HttpDriver.Controllers.Sockets.Models
{
    public class Tracker
    {
        private readonly Dictionary<ulong, Entity> _entities;
        private readonly IMemoryService _service;

        #region Constructors

        public Tracker(IMemoryService service)
        {
            _entities = new Dictionary<ulong, Entity>();
            _service = service;
        }

        #endregion

        #region Methods

        public void Receive(object packet)
        {
            switch (packet)
            {
                case EntityChange change:
                    if (!_entities.TryGetValue(change.Address, out var entity)) break;
                    foreach (var child in change.Changes) entity.Receive(child);
                    break;
                case EntityCreate create:
                    if (_entities.ContainsKey(create.Address)) break;
                    _entities[create.Address] = new Entity(create, _service);
                    break;
                case EntityDelete delete:
                    _entities.Remove(delete.Address);
                    break;
            }
        }

        public EntityUpdate? Update()
        {
            var entities = _entities.Values
                .Select(x => x.Update())
                .Where(x => x != null)
                .Select(x => x!)
                .ToList();
            return entities.Count != 0
                ? new EntityUpdate { Entities = entities }
                : null;
        }

        #endregion
    }
}