using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.AmenityAggregate;

namespace Property.Infrastructure.Configurations;

public class AmenityGroupConfiguration:IEntityTypeConfiguration<AmenityGroup>
{
    public void Configure(EntityTypeBuilder<AmenityGroup> builder)
    {
        builder.ToTable("AmenityGroups");

        builder.HasKey(ag => ag.Id);
        builder.Property(ag => ag.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
    }
}