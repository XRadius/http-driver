public class MemoryService {
  public unsafe bool Read(int pid, ulong address, byte[] buffer) {
    fixed (void *pointer = buffer) {
      var localIo = new Iovec {iov_base = pointer, iov_len = buffer.Length};
      var remoteIo = new Iovec {iov_base = (void*) address, iov_len = buffer.Length};
      return Libc.process_vm_readv(pid, &localIo, 1, &remoteIo, 1, 0) == buffer.Length;
    }
  }

  public unsafe bool Write(int pid, ulong address, byte[] buffer) {
    fixed (void *pointer = buffer) {
      var localIo = new Iovec {iov_base = pointer, iov_len = buffer.Length};
      var remoteIo = new Iovec {iov_base = (void*) address, iov_len = buffer.Length};
      return Libc.process_vm_writev(pid, &localIo, 1, &remoteIo, 1, 0) == buffer.Length;
    }
  }
}
