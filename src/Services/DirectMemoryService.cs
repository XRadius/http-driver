using HttpDriver.Utilities;
using HttpDriver.Utilities.Linux;

namespace HttpDriver.Services
{
    public class DirectMemoryService : IMemoryService
    {
        private readonly int _pid;

        #region Constructors

        public DirectMemoryService(int pid)
        {
            _pid = pid;
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Implementation of IMemoryService

        public unsafe bool Read(ulong address, byte[] buffer)
        {
            fixed (void* pointer = buffer)
            {
                var localIo = new Iovec { iov_base = pointer, iov_len = buffer.Length };
                var remoteIo = new Iovec { iov_base = (void*)address, iov_len = buffer.Length };
                return Libc.process_vm_readv(_pid, &localIo, 1, &remoteIo, 1, 0) == buffer.Length;
            }
        }

        public unsafe bool Write(ulong address, byte[] buffer)
        {
            fixed (void* pointer = buffer)
            {
                var localIo = new Iovec { iov_base = pointer, iov_len = buffer.Length };
                var remoteIo = new Iovec { iov_base = (void*)address, iov_len = buffer.Length };
                return Libc.process_vm_writev(_pid, &localIo, 1, &remoteIo, 1, 0) == buffer.Length;
            }
        }

        #endregion
    }
}