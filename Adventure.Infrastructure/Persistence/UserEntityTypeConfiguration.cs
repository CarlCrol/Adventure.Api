using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adventure.Infrastructure.Persistence;

public class UserEntityTypeConfiguration : IEntityTypeConfiguration<Domain.User>
{
    public void Configure(EntityTypeBuilder<Domain.User> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Username).HasField("_username").UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}