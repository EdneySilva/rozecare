using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RozeCare.Domain.Entities;

namespace RozeCare.Infrastructure.Persistence.Configurations;

public class UserRefreshTokenConfiguration : IEntityTypeConfiguration<UserRefreshToken>
{
    public void Configure(EntityTypeBuilder<UserRefreshToken> builder)
    {
        builder.Property(x => x.Token).HasMaxLength(500);
        builder.HasIndex(x => new { x.UserId, x.Token }).IsUnique();
    }
}
