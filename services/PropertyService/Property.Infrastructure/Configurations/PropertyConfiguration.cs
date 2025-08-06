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
            rules.Property(hr=>hr.AgeRestriction).HasColumnName("AgeRestriction").IsRequired();
            rules.Property(hr=>hr.PartyAllowed).HasColumnName("PartyAllowed").IsRequired();
            rules.Property(hr=>hr.PetAllowed).HasColumnName("PetAllowed").IsRequired();
            rules.Property(hr=>hr.SmokingAllowed).HasColumnName("SmokingAllowed").IsRequired();
            rules.Property(hr => hr.CheckInTimeFrom).HasColumnName("CheckInTimeFrom").IsRequired();
            rules.Property(hr => hr.CheckInTimeUntil).HasColumnName("CheckInTimeUntil").IsRequired();
            rules.Property(hr => hr.CheckOutTimeUntil).HasColumnName("CheckOutTimeUntil").IsRequired();
        });

        builder.OwnsOne(p => p.Location, location =>
        {
            location.Property(l => l.PostCode).HasColumnName("PostCode").IsRequired();
            location.Property(l => l.Address).HasColumnName("Address").IsRequired();
            location.Property(l => l.City).HasColumnName("City").IsRequired();
            location.Property(l => l.Country).HasColumnName("Country").IsRequired();
        });
        
        // Configuring relationships
        builder.HasMany(p => p.Amenities)
            .WithOne()
            .HasForeignKey(pa => pa.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p=>p.RentalUnits)
            .WithOne()
            .HasForeignKey(ru => ru.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(p=>p.Type)
            .WithMany()
            .HasForeignKey(pt=>pt.PropertyTypeId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p=> p.Languages)
            .WithOne()
            .HasForeignKey(pl=>pl.PropertyId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}