using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.ValueObjects;

namespace Property.Infrastructure.Configurations;

public class PropertyConfiguration: IEntityTypeConfiguration<Domain.Aggregates.AggregateRoot.Property>
{
    public void Configure(EntityTypeBuilder<Domain.Aggregates.AggregateRoot.Property> builder)
    {
        builder.ToTable("Properties");
    
        builder.HasKey(p => p.Id);
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        // Config Value Objects
        builder.OwnsOne(p => p.Rules, rules =>
        {
            rules.Property(hr=>hr.AgeRestriction).IsRequired();
            rules.Property(hr=>hr.FloorNumber).IsRequired();
            rules.Property(hr=>hr.PartyAllowed).IsRequired();
            rules.Property(hr=>hr.PetAllowed).IsRequired();
            rules.Property(hr=>hr.SmokingAllowed).IsRequired();
            rules.Property(hr => hr.CheckInTimeFrom).IsRequired();
            rules.Property(hr => hr.CheckInTimeUntil).IsRequired();
            rules.Property(hr => hr.CheckOutTimeUntil).IsRequired();
            rules.Property(hr => hr.CheckInTimeFrom).IsRequired();
        });

        builder.OwnsOne(p => p.Location, location =>
        {
            location.Property(l => l.PostCode).IsRequired();
            location.Property(l => l.Address).IsRequired();
            location.Property(l => l.City).IsRequired();
            location.Property(l => l.Country).IsRequired();
        });
        
        builder.HasMany(p => p.Amenities)
            .WithOne()
            .HasForeignKey(pa => pa.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p=>p.RentalUnits)
            .WithOne()
            .HasForeignKey(ru => ru.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p=>p.Images)
            .WithOne()
            .HasForeignKey(i => i.EntityId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p=>p.Type)
            .WithMany()
            .HasForeignKey(pt=>pt.PropertyTypeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}