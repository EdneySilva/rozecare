namespace RozeCare.Infrastructure.Options;

public class BlobStorageOptions
{
    public string? ConnectionString { get; set; }
    public string Container { get; set; } = "roze-docs";
}
