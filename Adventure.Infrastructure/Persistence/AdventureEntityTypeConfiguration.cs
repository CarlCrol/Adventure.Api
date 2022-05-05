using Adventure.Domain.ValueTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Adventure.Infrastructure.Persistence;

public class AdventureEntityTypeConfiguration : IEntityTypeConfiguration<Domain.Adventure>
{
    public void Configure(EntityTypeBuilder<Domain.Adventure> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title).HasField("_title").UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(x => x.Routes).HasField("_routes").HasConversion(
            v => JsonConvert.SerializeObject(v),  
            v => JsonConvert.DeserializeObject<List<Route>>(v));
    }
}