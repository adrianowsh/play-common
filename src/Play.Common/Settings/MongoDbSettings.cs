namespace Play.Common.Settings;

public sealed record MongoDbSettings
{
    public string Host { get; init; }
    public int Port { get; init; }
    public string ConnectionString => $"mongodb://{Host}:{Port}";
}
