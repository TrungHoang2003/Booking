using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.AggregateRoot;

namespace Property.Infrastructure.Configurations;

public class PropertyTypeConfiguration: IEntityTypeConfiguration<PropertyType>
{
    public void Configure(EntityTypeBuilder<PropertyType> builder)
    {
        builder.ToTable("PropertyTypes");

        builder.HasKey(pt => pt.Id);
        builder.Property(pt => pt.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(pt => pt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pt => pt.Description)
            .HasMaxLength(500);
    }
}