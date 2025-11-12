using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Application.Documents.Queries;

public record DocumentMetadataDto(Guid Id, string FileName, string ContentType, long Size, DateTime UploadedAt, IReadOnlyList<string> Tags, string? Description, string Hash, string BlobUrl);

public record GetDocumentsQuery(Guid PatientId) : IRequest<IReadOnlyList<DocumentMetadataDto>>;

public class GetDocumentsQueryHandler : IRequestHandler<GetDocumentsQuery, IReadOnlyList<DocumentMetadataDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetDocumentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<DocumentMetadataDto>> Handle(GetDocumentsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Documents.AsNoTracking()
            .Where(d => d.PatientId == request.PatientId)
            .ProjectTo<DocumentMetadataDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
