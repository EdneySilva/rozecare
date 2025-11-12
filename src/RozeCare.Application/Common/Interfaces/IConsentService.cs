namespace RozeCare.Application.Common.Interfaces;

public interface IConsentService
{
    Task<bool> HasConsentAsync(Guid patientId, Guid actorId, string scope, CancellationToken cancellationToken = default);
}
