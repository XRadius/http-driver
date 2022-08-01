using System.Runtime.InteropServices;

namespace HttpDriver.Utilities.Linux
{
    [StructLayout(LayoutKind.Sequential)]
    public unsafe struct Iovec
    {
        public void* iov_base;
        public int iov_len;
    }
}