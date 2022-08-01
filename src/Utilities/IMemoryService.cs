namespace HttpDriver.Utilities
{
    public interface IMemoryService : IDisposable
    {
        #region Methods

        bool Read(ulong address, byte[] buffer);

        bool Write(ulong address, byte[] buffer);

        #endregion
    }
}