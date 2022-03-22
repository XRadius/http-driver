using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public unsafe struct Iovec {
  public void *iov_base;
  public int iov_len;
}
