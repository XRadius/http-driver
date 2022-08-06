namespace HttpDriver.Controllers.Sockets.Extensions
{
    public static class StreamExtensions
    {
        #region Statics

        public static uint ReadVariableLength(this BinaryReader stream)
        {
            var more = true;
            var value = 0u;
            var shift = 0;

            while (more)
            {
                var chunk = stream.ReadByte();
                more = (chunk & 0x80u) != 0;
                value |= (chunk & 0x7Fu) << shift;
                shift += 7;
            }

            return value;
        }

        public static void WriteVariableLength(this BinaryWriter stream, uint value)
        {
            var more = true;

            while (more)
            {
                var chunk = (byte)(value & 0x7Fu);
                value >>= 7;
                more = value != 0;
                chunk |= (byte)(more ? 0x80u : 0);
                stream.Write(chunk);
            }
        }

        #endregion
    }
}