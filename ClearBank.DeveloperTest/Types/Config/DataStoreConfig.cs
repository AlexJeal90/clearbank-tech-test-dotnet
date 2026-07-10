using System.Reflection.Metadata;

namespace ClearBank.DeveloperTest.Types.Config;

public sealed record DataStoreConfig
{
    public const string ConfigSection = "DataStore";

    public DataStoreType DataStoreType { get; init; }
}
