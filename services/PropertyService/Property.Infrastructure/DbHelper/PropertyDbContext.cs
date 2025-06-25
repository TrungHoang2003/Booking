using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Property.Domain.Aggregates.AggregateRoot;
using Property.Domain.Aggregates.AmenityAggregate;
using Property.Domain.Aggregates.BedroomAggregate;
using Property.Domain.Aggregates.ImageAggregate;
using Property.Domain.Aggregates.RentalUnitAggregate;

namespace Property.Infrastructure.DbHelper;

public class PropertyDbContext(DbContextOptions<PropertyDbContext> options): DbContext(options)
{
    public DbSet<Domain.Aggregates.AggregateRoot.Property> Properties { get; set; }
    public DbSet<Amenity> Amenities { get; set; }
    public DbSet<AmenityGroup> AmenityGroups { get; set; }
    public DbSet<RentalUnit> RentalUnits { get; set; }
    public DbSet<PropertyAmenity> PropertyAmenities { get; set; }
    public DbSet<RentalUnitAmenity> RentalUnitAmenities { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Bedroom> Bedrooms { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Áp dụng tất cả các class kế thừa từ IEntityTypeConfiguration<> trong thư mục Configurations
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); 
    }
}