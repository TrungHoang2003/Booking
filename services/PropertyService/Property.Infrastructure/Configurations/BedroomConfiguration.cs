using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.BedroomAggregate;

namespace Property.Infrastructure.Configurations;

public class BedroomConfiguration : IEntityTypeConfiguration<Bedroom>
{
    public void Configure(EntityTypeBuilder<Bedroom> builder)
    {
        builder.ToTable("Bedrooms");

        builder.HasKey(b => b.Id);
        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.OwnsOne(b => b.Type, b =>
        {
            b.Property(bt => bt.Value).IsRequired();
        });
    }
}