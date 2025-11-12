using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RozeCare.Application.Documents.Commands;
using RozeCare.Application.Documents.Queries;

namespace RozeCare.Api.Controllers;

[ApiController]
[Route("api/patients/{patientId:guid}/documents")]
public class DocumentsController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Authorize(Roles = "Clinician,Patient,OrgAdmin,Admin")]
    [RequestSizeLimit(25_000_000)]
    public async Task<ActionResult<Guid>> Post(Guid patientId, [FromForm] IFormFile file, [FromForm] string? description, [FromForm] string? tags, CancellationToken cancellationToken)
    {
        if (file is null)
        {
            return BadRequest();
        }

        using var stream = file.OpenReadStream();
        var id = await _mediator.Send(new UploadDocumentCommand(patientId, file.FileName, file.ContentType, stream, (tags ?? string.Empty).Split(',', StringSplitOptions.RemoveEmptyEntries), description), cancellationToken);
        return CreatedAtAction(nameof(Get), new { patientId }, id);
    }

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IReadOnlyList<DocumentMetadataDto>>> Get(Guid patientId, CancellationToken cancellationToken)
    {
        var docs = await _mediator.Send(new GetDocumentsQuery(patientId), cancellationToken);
        return Ok(docs);
    }

    [HttpGet("{documentId:guid}/download")]
    [Authorize]
    public async Task<ActionResult<string>> Download(Guid patientId, Guid documentId, CancellationToken cancellationToken)
    {
        var link = await _mediator.Send(new GetDocumentDownloadLinkQuery(patientId, documentId), cancellationToken);
        return Ok(new { url = link });
    }
}
