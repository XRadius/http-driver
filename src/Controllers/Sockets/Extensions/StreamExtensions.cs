using HttpDriver.Controllers.Sockets.Abstracts.Interfaces;

namespace HttpDriver.Controllers.Sockets.Extensions
{
    public static class StreamExtensions
    {
        #region Statics

        public static byte[] ReadByteArray(this BinaryReader stream)
        {
            var size = stream.ReadUInt16();
            var buffer = stream.ReadBytes(size);
            return buffer;
        }

        public static T[] ReadEntityArray<T>(this BinaryReader stream, Func<BinaryReader, T> factory)
        {
            var size = stream.ReadUInt16();
            var items = new T[size];
            for (var i = 0; i < size; i++) items[i] = factory(stream);
            return items;
        }

        public static void WriteByteArray(this BinaryWriter stream, byte[] buffer)
        {
            stream.Write((ushort)buffer.Length);
            stream.Write(buffer);
        }

        public static void WriteEntityArray<T>(this BinaryWriter stream, ICollection<T> items) where T : IPacketWriter
        {
            stream.Write((ushort)items.Count);
            foreach (var item in items) item.Write(stream);
        }

        #endregion
    }
}