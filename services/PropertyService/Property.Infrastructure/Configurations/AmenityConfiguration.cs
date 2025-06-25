using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.AmenityAggregate;

namespace Property.Infrastructure.Configurations;

public class AmenityConfiguration:IEntityTypeConfiguration<Amenity>
{
    public void Configure(EntityTypeBuilder<Amenity> builder)
    {
        builder.ToTable("Amenities");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        // Config Value Objects
        builder.OwnsOne(a => a.Price);
    }
}