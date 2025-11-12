using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RozeCare.Domain.Entities;

namespace RozeCare.Infrastructure.Persistence.Configurations;

public class PatientProfileConfiguration : IEntityTypeConfiguration<PatientProfile>
{
    public void Configure(EntityTypeBuilder<PatientProfile> builder)
    {
        builder.Property(p => p.Conditions)
            .HasConversion(
                v => string.Join(';', v),
                v => string.IsNullOrEmpty(v) ? new List<string>() : v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(p => p.Allergies)
            .HasConversion(
                v => string.Join(';', v),
                v => string.IsNullOrEmpty(v) ? new List<string>() : v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(p => p.PreferredProviders)
            .HasConversion(
                v => string.Join(';', v),
                v => string.IsNullOrEmpty(v) ? new List<string>() : v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(p => p.EmergencyContacts)
            .HasConversion(
                v => string.Join(';', v),
                v => string.IsNullOrEmpty(v) ? new List<string>() : v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());
    }
}
