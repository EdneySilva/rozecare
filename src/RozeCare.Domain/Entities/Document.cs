using RozeCare.Domain.Common;

namespace RozeCare.Domain.Entities;

public class Document : BaseAuditableEntity
{
    public Guid PatientId { get; set; }
        = Guid.Empty;

    public ApplicationUser? Patient { get; set; }
        = null;

    public string BlobUrl { get; set; } = string.Empty;

    public string FileName { get; set; } = string.Empty;

    public string ContentType { get; set; } = string.Empty;

    public long Size { get; set; }
        = 0;

    public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    public List<string> Tags { get; set; } = new();

    public string? Description { get; set; }
        = null;

    public string Hash { get; set; } = string.Empty;
}
