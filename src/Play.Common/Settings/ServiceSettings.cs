namespace Play.Common.Settings;
public sealed record ServiceSettings
{
    public string ServiceName { get; init; }
    public string Authority { get; init; }
}
