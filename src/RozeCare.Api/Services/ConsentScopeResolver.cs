using Microsoft.AspNetCore.Routing;

namespace RozeCare.Api.Services;

public class ConsentScopeResolver
{
    private readonly Dictionary<string, string> _routeScopes = new(StringComparer.OrdinalIgnoreCase)
    {
        ["PatientsController.GetPatientById"] = Application.Common.Models.ConsentScopes.ProfileRead,
        ["ObservationsController.Get"] = Application.Common.Models.ConsentScopes.ObservationRead,
        ["ObservationsController.Post"] = Application.Common.Models.ConsentScopes.ObservationWrite,
        ["MedicationsController.Get"] = Application.Common.Models.ConsentScopes.MedicationRead,
        ["MedicationsController.Post"] = Application.Common.Models.ConsentScopes.MedicationWrite,
        ["AllergiesController.Get"] = Application.Common.Models.ConsentScopes.AllergyRead,
        ["AllergiesController.Post"] = Application.Common.Models.ConsentScopes.AllergyWrite,
        ["DocumentsController.Get"] = Application.Common.Models.ConsentScopes.DocumentRead,
        ["DocumentsController.Post"] = Application.Common.Models.ConsentScopes.DocumentWrite,
        ["DocumentsController.Download"] = Application.Common.Models.ConsentScopes.DocumentRead,
        ["EncountersController.Get"] = Application.Common.Models.ConsentScopes.EncounterRead,
        ["EncountersController.Post"] = Application.Common.Models.ConsentScopes.EncounterWrite
    };

    public string? Resolve(string controller, string action)
    {
        var key = $"{controller}.{action}";
        return _routeScopes.TryGetValue(key, out var scope) ? scope : null;
    }
}
