namespace HttpDriver.Apis.DataModels
{
    [Flags]
    public enum MapEntryPermissions
    {
        None,
        Read,
        Write,
        Execute,
        Shared
    }
}