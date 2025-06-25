using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Domain.ValueObjects;

namespace Property.Infrastructure.Configurations;

public class RentalUnitConfiguration:IEntityTypeConfiguration<RentalUnit>
{
    public void Configure(EntityTypeBuilder<RentalUnit> builder)
    {
        builder.ToTable("RentalUnits"); 
       
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
         
        // Config TPH (Table Per Hierarchy)
        builder.HasDiscriminator<string>("RentalType")
            .HasValue<RoomRentalUnit>("RoomBased")
            .HasValue<EntirePropertyRentalUnit>("EntireProperty");

        builder.Property<bool>("SharedBathroom").HasColumnName("SharedBathroom");
        builder.Property<int>("Size").HasColumnName("Size");
        builder.Property<int>("BedroomsCount").HasColumnName("BedroomsCount");
        builder.Property<int>("BathroomsCount").HasColumnName("BathroomsCount");
         
        // Config Value Objects
        builder.OwnsOne(r => r.BasePricePerNight);

        builder.HasMany(r => r.Amenities)
            .WithOne()
            .HasForeignKey(ra => ra.RentalUnitId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(r => r.Images)
            .WithOne()
            .HasForeignKey(i => i.EntityId);

        builder.HasMany(r => r.Bedrooms)
            .WithOne()
            .HasForeignKey(b => b.RentalUnitId);
    }
}