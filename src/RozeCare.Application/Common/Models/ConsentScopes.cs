namespace RozeCare.Application.Common.Models;

public static class ConsentScopes
{
    public const string ProfileRead = "profile:read";
    public const string ProfileWrite = "profile:write";
    public const string ObservationRead = "obs:read";
    public const string ObservationWrite = "obs:write";
    public const string MedicationRead = "med:read";
    public const string MedicationWrite = "med:write";
    public const string AllergyRead = "allergy:read";
    public const string AllergyWrite = "allergy:write";
    public const string DocumentRead = "docs:read";
    public const string DocumentWrite = "docs:write";
    public const string EncounterRead = "enc:read";
    public const string EncounterWrite = "enc:write";
}
