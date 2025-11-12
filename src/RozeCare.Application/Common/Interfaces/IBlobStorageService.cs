namespace RozeCare.Application.Common.Interfaces;

public interface IBlobStorageService
{
    Task<string> UploadAsync(string container, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default);

    Task<Stream> DownloadAsync(string container, string blobName, CancellationToken cancellationToken = default);

    Task<string> GenerateDownloadUrlAsync(string container, string blobName, TimeSpan lifetime, CancellationToken cancellationToken = default);
}
