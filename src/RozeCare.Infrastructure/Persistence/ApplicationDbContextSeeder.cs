using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RozeCare.Domain.Entities;
using RozeCare.Domain.Enums;

namespace RozeCare.Infrastructure.Persistence;

public static class ApplicationDbContextSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        await context.Database.MigrateAsync();

        if (!context.Roles.Any())
        {
            foreach (var role in Enum.GetNames<UserRole>())
            {
                context.Roles.Add(new IdentityRole<Guid>(role));
            }
            await context.SaveChangesAsync();
        }

        if (!context.Users.Any())
        {
            var patient = new ApplicationUser
            {
                UserName = "patient@rozecare.test",
                Email = "patient@rozecare.test",
                Name = "Ana Paciente",
                Role = UserRole.Patient
            };
            await userManager.CreateAsync(patient, "P@ssword123!");
            await userManager.AddToRoleAsync(patient, UserRole.Patient.ToString());

            var clinician = new ApplicationUser
            {
                UserName = "clinician@rozecare.test",
                Email = "clinician@rozecare.test",
                Name = "Dr. Joao",
                Role = UserRole.Clinician
            };
            await userManager.CreateAsync(clinician, "P@ssword123!");
            await userManager.AddToRoleAsync(clinician, UserRole.Clinician.ToString());

            context.PatientProfiles.Add(new PatientProfile
            {
                UserId = patient.Id,
                BloodType = "O+",
                Conditions = new() { "Hipertensão" },
                Allergies = new() { "Penicilina" },
                EmergencyContacts = new() { "Maria: +55-11-9999-8888" },
                PreferredProviders = new() { "Hospital Central" }
            });

            var provider = new Provider
            {
                Name = "Hospital Central",
                Type = ProviderType.Hospital,
                Address = "Av. Paulista, 1000",
                Contact = "+55-11-5555-0000",
                Accreditation = "CRM-SP"
            };
            context.Providers.Add(provider);
            await context.SaveChangesAsync();

            context.Observations.Add(new Observation
            {
                PatientId = patient.Id,
                Code = "heart-rate",
                Display = "Heart Rate",
                ValueQuantity = 72,
                Unit = "bpm",
                EffectiveDate = DateTime.UtcNow.AddDays(-2)
            });
            context.Observations.Add(new Observation
            {
                PatientId = patient.Id,
                Code = "blood-pressure",
                Display = "Blood Pressure",
                ValueString = "120/80",
                EffectiveDate = DateTime.UtcNow.AddDays(-5)
            });

            context.Medications.Add(new Medication
            {
                PatientId = patient.Id,
                Name = "Atorvastatina",
                Dosage = "10mg",
                StartDate = DateTime.UtcNow.AddMonths(-2),
                PrescribedBy = "Dr. Joao"
            });

            context.Allergies.Add(new Allergy
            {
                PatientId = patient.Id,
                Substance = "Amendoim",
                Reaction = "Urticária",
                Severity = "Alta"
            });

            context.Consents.Add(new Consent
            {
                PatientId = patient.Id,
                GranteeType = ConsentGranteeType.Provider,
                GranteeId = provider.Id,
                Scopes = new() { "obs:read", "docs:read" },
                ExpiresAtUtc = DateTime.UtcNow.AddMonths(3),
                Status = ConsentStatus.Active
            });

            context.Documents.Add(new Document
            {
                PatientId = patient.Id,
                FileName = "example.pdf",
                ContentType = "application/pdf",
                BlobUrl = "https://storage.example.com/fake/example.pdf",
                Size = 1024,
                Tags = new() { "lab" },
                Description = "Exame de sangue",
                Hash = Guid.NewGuid().ToString("N")
            });

            await context.SaveChangesAsync();
        }
    }
}
