using System.Diagnostics;
using HttpDriver.Utilities;

namespace HttpDriver.Services
{
    public class GdbMemoryService : IMemoryService
    {
        private readonly string _path;
        private readonly int _pid;
        private readonly object _syncRoot;

        #region Constructors

        public GdbMemoryService(int pid)
        {
            _path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
            _pid = pid;
            _syncRoot = new object();
        }

        #endregion

        #region Methods

        private bool Dump(ulong from, ulong to)
        {
            using var process = Process.Start(new ProcessStartInfo
            {
                FileName = "gdb",
                ArgumentList =
                {
                    "-batch-silent",
                    "-pid", _pid.ToString(),
                    "-ex", $"dump binary memory {_path} 0x{from:x} 0x{to:x}",
                    "-ex", "quit"
                }
            });
            process?.WaitForExit();
            return process?.ExitCode == 0;
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            lock (_syncRoot)
            {
                if (File.Exists(_path))
                {
                    File.Delete(_path);
                }

                GC.SuppressFinalize(this);
            }
        }

        #endregion

        #region Implementation of IMemoryService

        public bool Read(ulong address, byte[] buffer)
        {
            lock (_syncRoot)
            {
                if (!Dump(address, address + (uint)buffer.Length) || !File.Exists(_path)) return false;
                var result = File.ReadAllBytes(_path);
                if (result.Length != buffer.Length) return false;
                Buffer.BlockCopy(result, 0, buffer, 0, buffer.Length);
                return true;
            }
        }

        public bool Write(ulong address, byte[] buffer)
        {
            return false;
        }

        #endregion
    }
}