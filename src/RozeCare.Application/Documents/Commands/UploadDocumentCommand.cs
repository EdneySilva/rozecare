using System.Security.Cryptography;
using FluentValidation;
using MediatR;
using RozeCare.Application.Common.Interfaces;
using RozeCare.Domain.Entities;

namespace RozeCare.Application.Documents.Commands;

public record UploadDocumentCommand(Guid PatientId, string FileName, string ContentType, Stream Content, IReadOnlyCollection<string> Tags, string? Description) : IRequest<Guid>;

public class UploadDocumentCommandValidator : AbstractValidator<UploadDocumentCommand>
{
    public UploadDocumentCommandValidator()
    {
        RuleFor(x => x.PatientId).NotEmpty();
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.ContentType).NotEmpty();
        RuleFor(x => x.Content).NotNull();
    }
}

public class UploadDocumentCommandHandler : IRequestHandler<UploadDocumentCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly IBlobStorageService _blobStorageService;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly IAuditService _auditService;

    public UploadDocumentCommandHandler(
        IApplicationDbContext context,
        IBlobStorageService blobStorageService,
        IDateTimeProvider dateTimeProvider,
        IAuditService auditService)
    {
        _context = context;
        _blobStorageService = blobStorageService;
        _dateTimeProvider = dateTimeProvider;
        _auditService = auditService;
    }

    public async Task<Guid> Handle(UploadDocumentCommand request, CancellationToken cancellationToken)
    {
        using var memory = new MemoryStream();
        await request.Content.CopyToAsync(memory, cancellationToken);
        memory.Position = 0;

        string hash;
        using (var sha = SHA256.Create())
        {
            hash = Convert.ToHexString(sha.ComputeHash(memory.ToArray()));
        }

        memory.Position = 0;
        var blobName = $"{request.PatientId}/{Guid.NewGuid()}-{request.FileName}";
        var blobUrl = await _blobStorageService.UploadAsync("roze-docs", blobName, memory, request.ContentType, cancellationToken);

        var document = new Document
        {
            PatientId = request.PatientId,
            FileName = request.FileName,
            ContentType = request.ContentType,
            Size = memory.Length,
            Tags = request.Tags.ToList(),
            Description = request.Description,
            BlobUrl = blobUrl,
            Hash = hash,
            UploadedAt = _dateTimeProvider.UtcNow
        };

        _context.Documents.Add(document);
        await _context.SaveChangesAsync(cancellationToken);
        await _auditService.LogAsync(null, "document.upload", nameof(Document), document.Id, new { document.PatientId, document.FileName }, cancellationToken);
        return document.Id;
    }
}
