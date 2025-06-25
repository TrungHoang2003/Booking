using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.BedroomAggregate;

namespace Property.Infrastructure.Configurations;

public class BedroomConfiguration:IEntityTypeConfiguration<Bedroom>
{
    public void Configure(EntityTypeBuilder<Bedroom> builder)
    {
        builder.ToTable("Bedrooms");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        // Config Value Objects
        builder.OwnsOne(b => b.CotPrice, price=>
        {
            price.Property(cp=>cp.Amount).IsRequired();
            price.Property(cp=>cp.Currency).IsRequired();
        });

        builder.OwnsOne(b => b.Type, bedType =>
            {
               bedType.Property<string>("Value").HasColumnName("Type").IsRequired(); 
            }
        );
    }
}