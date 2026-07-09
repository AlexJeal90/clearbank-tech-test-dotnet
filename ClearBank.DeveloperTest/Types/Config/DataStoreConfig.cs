namespace ClearBank.DeveloperTest.Types.Config;

public sealed record DataStoreConfig
{
    public DataStoreType DataStoreType { get; init; }
}
