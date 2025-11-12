using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RozeCare.Domain.Entities;
using RozeCare.Domain.Enums;

namespace RozeCare.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.Name).HasMaxLength(200);
        builder.Property(u => u.Role)
            .HasConversion<string>()
            .HasMaxLength(50);
        builder.Property(u => u.Country)
            .HasConversion(
                v => v != null ? v.Value : null,
                v => v != null ? Domain.ValueObjects.CountryCode.Create(v) : null)
            .HasMaxLength(3);
        builder.Property(u => u.PhoneNumber)
            .HasMaxLength(30);
    }
}
