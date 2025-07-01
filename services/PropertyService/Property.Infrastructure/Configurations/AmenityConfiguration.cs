using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.AmenityAggregate;
using Property.Domain.ValueObjects;

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

        builder.HasData(
            new Amenity(1, "Air conditioning", null, null,  false),
            new Amenity(2, "Heating", null, null,  false),
            new Amenity(3, "Free Wifi", null, null,  false),
            new Amenity(4, "Electric vehicle charing station", null, null,  false),
            new Amenity(5, "Free parking on premises", null, null,  false)
        );
    }
}