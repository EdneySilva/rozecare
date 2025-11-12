using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RozeCare.Domain.Entities;
using RozeCare.Domain.Enums;

namespace RozeCare.Infrastructure.Persistence.Configurations;

public class ConsentConfiguration : IEntityTypeConfiguration<Consent>
{
    public void Configure(EntityTypeBuilder<Consent> builder)
    {
        builder.Property(c => c.GranteeType)
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(c => c.Scopes)
            .HasConversion(
                v => string.Join(';', v),
                v => string.IsNullOrEmpty(v) ? new List<string>() : v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList());
        builder.HasIndex(c => new { c.PatientId, c.Status });
    }
}
