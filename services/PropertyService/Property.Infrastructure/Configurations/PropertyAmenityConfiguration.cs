using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.AggregateRoot;

namespace Property.Infrastructure.Configurations;

public class PropertyAmenityConfiguration:IEntityTypeConfiguration<PropertyAmenity>
{
    public void Configure(EntityTypeBuilder<PropertyAmenity> builder)
    {
        builder.ToTable("PropertyAmenities");

        builder.HasKey(pa => pa.Id);
        builder.Property(pa => pa.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.OwnsOne(pa => pa.Price, price =>
        {
            price.Property(p => p.Amount).HasColumnName("Amount").IsRequired();
            price.Property(p => p.Currency).HasColumnName("Currency").IsRequired();
        });
    }
}