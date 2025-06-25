using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.RentalUnitAggregate;

namespace Property.Infrastructure.Configurations;

public class RentalUnitAmenityConfiguration: IEntityTypeConfiguration<RentalUnitAmenity>
{
    public void Configure(EntityTypeBuilder<RentalUnitAmenity> builder)
    {
        builder.ToTable("RentalUnitAmenities");

        builder.HasKey(rua => rua.Id);
        builder.Property(rua => rua.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
    }
}