using AutoMapper;
using RozeCare.Application.Patients.Queries;
using RozeCare.Application.Observations.Queries;
using RozeCare.Application.Documents.Queries;
using RozeCare.Application.Consents.Queries;
using RozeCare.Application.Providers.Queries;
using RozeCare.Application.Audit.Queries;
using RozeCare.Application.Fhir.Queries;
using RozeCare.Domain.Entities;

namespace RozeCare.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PatientProfile, PatientProfileDto>();
        CreateMap<Observation, ObservationDto>();
        CreateMap<Medication, Medications.Queries.MedicationDto>();
        CreateMap<Allergy, Allergies.Queries.AllergyDto>();
        CreateMap<Document, DocumentMetadataDto>();
        CreateMap<Consent, ConsentDto>();
        CreateMap<Provider, ProviderDto>();
        CreateMap<AuditLog, AuditLogDto>();
        CreateMap<Observation, FhirObservationDto>();
        CreateMap<Encounter, Encounters.Queries.EncounterDto>();
    }
}
