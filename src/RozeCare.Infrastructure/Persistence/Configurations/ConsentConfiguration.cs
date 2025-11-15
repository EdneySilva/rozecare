using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RozeCare.Domain.Entities;
using RozeCare.Domain.Enums;

namespace RozeCare.Infrastructure.Persistence.Configurations;

public sealed class ConsentConfiguration : IEntityTypeConfiguration<Consent>
{
    public void Configure(EntityTypeBuilder<Consent> builder)
    {
        builder.Property(c => c.GranteeType)
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.Status)
            .HasConversion<string>()
            .HasMaxLength(50);

        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        var listToJsonConverter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, jsonOptions),
            v => string.IsNullOrWhiteSpace(v)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(v, jsonOptions)!);

        builder.Property(c => c.Scopes)
            .HasColumnType("jsonb")
            .HasConversion(listToJsonConverter);

        builder.HasIndex(c => new { c.PatientId, c.Status });
    }
}
