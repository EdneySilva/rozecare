using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Infrastructure.Options;

namespace RozeCare.Infrastructure.Services;

public class BlobStorageService : IBlobStorageService
{
    private readonly BlobStorageOptions _options;

    public BlobStorageService(IOptions<BlobStorageOptions> options)
    {
        _options = options.Value;
    }

    private BlobContainerClient GetClient()
    {
        var serviceClient = new BlobServiceClient(_options.ConnectionString);
        return serviceClient.GetBlobContainerClient(_options.Container);
    }

    public async Task<string> UploadAsync(string container, string blobName, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        var containerClient = GetClient();
        await containerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        var blobClient = containerClient.GetBlobClient(blobName);
        content.Position = 0;
        await blobClient.UploadAsync(content, overwrite: true, cancellationToken: cancellationToken);
        await blobClient.SetHttpHeadersAsync(new Azure.Storage.Blobs.Models.BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);
        return blobClient.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string container, string blobName, CancellationToken cancellationToken = default)
    {
        var containerClient = GetClient();
        var blobClient = containerClient.GetBlobClient(blobName);
        var response = await blobClient.DownloadAsync(cancellationToken);
        return response.Value.Content;
    }

    public Task<string> GenerateDownloadUrlAsync(string container, string blobName, TimeSpan lifetime, CancellationToken cancellationToken = default)
    {
        var containerClient = GetClient();
        var blobClient = containerClient.GetBlobClient(blobName);
        if (!blobClient.CanGenerateSasUri)
        {
            return Task.FromResult(blobClient.Uri.ToString());
        }

        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = containerClient.Name,
            BlobName = blobName,
            Resource = "b",
            ExpiresOn = DateTimeOffset.UtcNow.Add(lifetime)
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        var sasUri = blobClient.GenerateSasUri(sasBuilder);
        return Task.FromResult(sasUri.ToString());
    }
}
