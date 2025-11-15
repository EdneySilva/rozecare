using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RozeCare.Domain.Entities;

namespace RozeCare.Infrastructure.Persistence.Configurations;

public sealed class PatientProfileConfiguration : IEntityTypeConfiguration<PatientProfile>
{
    public void Configure(EntityTypeBuilder<PatientProfile> builder)
    {
        builder.ToTable("PatientProfiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.BloodType)
            .HasMaxLength(8);

        var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

        var listToJsonConverter = new ValueConverter<List<string>, string>(
            v => JsonSerializer.Serialize(v, jsonOptions),
            v => string.IsNullOrWhiteSpace(v)
                ? new List<string>()
                : JsonSerializer.Deserialize<List<string>>(v, jsonOptions)!);

        builder.Property(x => x.Conditions)
            .HasColumnType("jsonb")
            .HasConversion(listToJsonConverter);

        builder.Property(x => x.Allergies)
            .HasColumnType("jsonb")
            .HasConversion(listToJsonConverter);

        builder.Property(x => x.PreferredProviders)
            .HasColumnType("jsonb")
            .HasConversion(listToJsonConverter);

        builder.Property(x => x.EmergencyContacts)
            .HasColumnType("jsonb")
            .HasConversion(listToJsonConverter);
    }
}
