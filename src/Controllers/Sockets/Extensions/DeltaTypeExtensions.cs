using HttpDriver.Controllers.Sockets.Abstracts.Enums;

namespace HttpDriver.Controllers.Sockets.Extensions
{
    public static class DeltaTypeExtensions
    {
        #region Statics

        public static int Size(this DeltaType type)
        {
            return type switch
            {
                DeltaType.Byte => 1,
                DeltaType.Int16 => 2,
                DeltaType.Int32 => 4,
                DeltaType.Int64 => 8,
                DeltaType.Float32 => 4,
                DeltaType.Float64 => 8,
                DeltaType.UInt16 => 2,
                DeltaType.UInt32 => 4,
                DeltaType.UInt64 => 8,
                _ => throw new Exception()
            };
        }

        public static byte[] Transform(this DeltaType type, byte[] current, byte[] delta)
        {
            return type switch
            {
                DeltaType.Byte => new[] { (byte)(Convert.ToByte(current) + Convert.ToByte(delta)) },
                DeltaType.Int16 => BitConverter.GetBytes((short)(BitConverter.ToInt16(current) + BitConverter.ToInt16(delta))),
                DeltaType.Int32 => BitConverter.GetBytes(BitConverter.ToInt32(current) + BitConverter.ToInt32(delta)),
                DeltaType.Int64 => BitConverter.GetBytes(BitConverter.ToInt64(current) + BitConverter.ToInt64(delta)),
                DeltaType.Float32 => BitConverter.GetBytes(BitConverter.ToSingle(current) + BitConverter.ToSingle(delta)),
                DeltaType.Float64 => BitConverter.GetBytes(BitConverter.ToDouble(current) + BitConverter.ToDouble(delta)),
                DeltaType.UInt16 => BitConverter.GetBytes((ushort)(BitConverter.ToUInt16(current) + BitConverter.ToUInt16(delta))),
                DeltaType.UInt32 => BitConverter.GetBytes(BitConverter.ToUInt32(current) + BitConverter.ToUInt32(delta)),
                DeltaType.UInt64 => BitConverter.GetBytes(BitConverter.ToUInt64(current) + BitConverter.ToUInt64(delta)),
                _ => throw new Exception()
            };
        }

        #endregion
    }
}