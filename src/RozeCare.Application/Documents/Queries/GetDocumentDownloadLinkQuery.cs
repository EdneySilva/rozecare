using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Documents.Queries;

public record GetDocumentDownloadLinkQuery(Guid PatientId, Guid DocumentId) : IRequest<string>;

public class GetDocumentDownloadLinkQueryHandler : IRequestHandler<GetDocumentDownloadLinkQuery, string>
{
    private readonly IApplicationDbContext _context;
    private readonly IBlobStorageService _blobStorageService;

    public GetDocumentDownloadLinkQueryHandler(IApplicationDbContext context, IBlobStorageService blobStorageService)
    {
        _context = context;
        _blobStorageService = blobStorageService;
    }

    public async Task<string> Handle(GetDocumentDownloadLinkQuery request, CancellationToken cancellationToken)
    {
        var document = await _context.Documents.AsNoTracking()
            .FirstOrDefaultAsync(d => d.PatientId == request.PatientId && d.Id == request.DocumentId, cancellationToken)
            ?? throw new InvalidOperationException("Document not found.");

        var uri = new Uri(document.BlobUrl);
        var blobName = uri.Segments.Skip(1).Aggregate(string.Empty, (acc, segment) => acc + segment);
        return await _blobStorageService.GenerateDownloadUrlAsync("roze-docs", blobName, TimeSpan.FromMinutes(5), cancellationToken);
    }
}
