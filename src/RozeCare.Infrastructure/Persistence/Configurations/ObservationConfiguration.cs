using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RozeCare.Domain.Entities;

namespace RozeCare.Infrastructure.Persistence.Configurations;

public class ObservationConfiguration : IEntityTypeConfiguration<Observation>
{
    public void Configure(EntityTypeBuilder<Observation> builder)
    {
        builder.HasIndex(o => new { o.PatientId, o.Code });
    }
}
