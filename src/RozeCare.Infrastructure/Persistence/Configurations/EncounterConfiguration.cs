using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RozeCare.Domain.Entities;

namespace RozeCare.Infrastructure.Persistence.Configurations;

public class EncounterConfiguration : IEntityTypeConfiguration<Encounter>
{
    public void Configure(EntityTypeBuilder<Encounter> builder)
    {
        builder.Property(e => e.Diagnoses)
            .HasConversion(
                v => string.Join(';', v),
                v => string.IsNullOrWhiteSpace(v) ? new List<string>() : v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());

        builder.Property(e => e.Prescriptions)
            .HasConversion(
                v => string.Join(';', v),
                v => string.IsNullOrWhiteSpace(v) ? new List<string>() : v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());
    }
}
