using System.Runtime.InteropServices;

public class Libc {
  [DllImport("libc")]
  public static extern unsafe int process_vm_readv(int pid, Iovec *local_iov, ulong liovcnt, Iovec *remote_iov, ulong riovcnt, ulong flags);

  [DllImport("libc")]
  public static extern unsafe int process_vm_writev(int pid, Iovec *local_iov, ulong liovcnt, Iovec *remote_iov, ulong riovcnt, ulong flags);
}
