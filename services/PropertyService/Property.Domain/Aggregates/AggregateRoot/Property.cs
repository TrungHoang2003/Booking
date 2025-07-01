using Property.Domain.Aggregates.ImageAggregate;
using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.AggregateRoot;

public class Property : BuildingBlocks.DomainDrivenPattern.AggregateRoot
{
    public int PropertyTypeId { get; private set; }
    public int HostId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public int? FloorNumber { get; private set; }
    public string? ThumbnailUrl { get; private set; }

    // Value Objects
    public HouseRule Rules { get; private set; }
    public Location Location { get; private set; }

    // Navigation Properties
    public PropertyType? Type;
    public List<PropertyAmenity> Amenities = [];
    public List<RentalUnit> RentalUnits = [];
    public List<Image> Images = [];

    //Constructors
    public Property(int propertyTypeId, int hostId, string name, string? description, int? floorNumber,
        string? thumbnailUrl, HouseRule rules, Location location)
    {
        PropertyTypeId = propertyTypeId;
        HostId = hostId;
        Name = name;
        Description = description;
        FloorNumber = floorNumber;
        ThumbnailUrl = thumbnailUrl;
        Rules = rules;
        Location = location;

        //AddDomainEvent(new PropertyCreatedDomainEvent(this.Id, propertyTypeId, hostId, name, description, floorNumber, rules, location, thumbnailUrl));
        if (hostId <= 0) throw new ArgumentException("hostId must be valid");
        if (propertyTypeId <= 0) throw new ArgumentException("propertyTypeId must be valid");
    }

    // Domain Business Logics
    public void AddRentalUnit(RentalUnit rentalUnit)
    {
        ArgumentNullException.ThrowIfNull(rentalUnit);

        RentalUnits.Add(rentalUnit);
        //AddDomainEvent(new RentalUnitAddedToPropertyDomainEvent(this.Id, rentalUnit.Id));
    }
    
    public void AddAmenity(PropertyAmenity propertyAmenity)
    {
        ArgumentNullException.ThrowIfNull(propertyAmenity);

        Amenities.Add(propertyAmenity);
        //AddDomainEvent(new PropertyAmenityAddedDomainEvent(this.Id, propertyAmenity.AmenityId));
    }
    
    public void AddDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or empty.", nameof(description));

        Description = description;
    }
}