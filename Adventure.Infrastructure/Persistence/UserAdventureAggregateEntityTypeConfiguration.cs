using Adventure.Domain.ValueTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;

namespace Adventure.Infrastructure.Persistence;

public class UserAdventureAggregateEntityTypeConfiguration : IEntityTypeConfiguration<Domain.UserAdventureAggregate>
{
    public void Configure(EntityTypeBuilder<Domain.UserAdventureAggregate> builder)
    {
        builder.HasKey(x => x.Id);

        builder.HasOne(x => x.User).WithOne();
        // builder.Property(x => x.User).HasField("_user").UsePropertyAccessMode(PropertyAccessMode.Field);
        // builder.Property(x => x.UserId).HasField("_userId").UsePropertyAccessMode(PropertyAccessMode.Field);
        
        builder.HasOne(x => x.Adventure).WithOne();
        //builder.Property(x => x.Adventure).HasField("_adventure").UsePropertyAccessMode(PropertyAccessMode.Field);
        // builder.Property(x => x.AdventureId).HasField("_adventureId").UsePropertyAccessMode(PropertyAccessMode.Field);

         builder.Property(x => x.AdventuredOn).HasField("_adventuredOn").UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(x => x.SelectedRoutes).HasField("_selectedRoutes").HasConversion(
            v => JsonConvert.SerializeObject(v),  
            v => JsonConvert.DeserializeObject<List<SelectedRoute>>(v));
    }
}