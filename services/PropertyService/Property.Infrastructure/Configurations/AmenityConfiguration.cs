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

        // Config Value Objects
        builder.OwnsOne(a => a.Price, price=>
        {
            price.Property(p => p.Amount).HasColumnName("Price").IsRequired();
            price.Property(p => p.Currency).HasColumnName("PriceCurrency").IsRequired();
        });
    }
}