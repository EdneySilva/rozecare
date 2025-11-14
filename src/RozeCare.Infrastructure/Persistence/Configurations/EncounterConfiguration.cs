using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RozeCare.Domain.Entities;

namespace RozeCare.Infrastructure.Persistence.Configurations;

public sealed class EncounterConfiguration : IEntityTypeConfiguration<Encounter>
{
    public void Configure(EntityTypeBuilder<Encounter> builder)
    {
        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        var listToJsonConverter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, jsonOptions),
            v => string.IsNullOrWhiteSpace(v)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(v, jsonOptions)!);

        builder.Property(e => e.Diagnoses)
            .HasColumnType("jsonb")
            .HasConversion(listToJsonConverter);

        builder.Property(e => e.Prescriptions)
            .HasColumnType("jsonb")
            .HasConversion(listToJsonConverter);
    }
}
