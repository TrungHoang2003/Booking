using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.AggregateRoot;

public class Property: BuildingBlocks.DomainDrivenPattern.AggregateRoot
{
    public int PropertyTypeId { get; private set; }
    public int HostId { get; private set; }
    public string? Name { get; private set; } 
    public string? Description { get; private set; }
    public int? FloorNumber { get; private set; }
    public string? ThumbnailUrl { get; set; } 
    
    // Value Objects
    public HouseRule Rules { get; private set; } 
    public Location Location{ get; private set; }
    
    // Navigation Properties
    private readonly List<RentalUnit> _rentalUnits = [];
    public IReadOnlyCollection<RentalUnit> RentalUnits => _rentalUnits.AsReadOnly();
    
    //Constructors
    public Property(int propertyTypeId, int hostId, string? name, string? description, int? floorNumber, HouseRule rules, Location location, string? thumbnailUrl)
    {
        PropertyTypeId = propertyTypeId;
        HostId = hostId;
        Name = name;
        Description = description;
        FloorNumber = floorNumber;
        Rules = rules;
        Location = location;
        ThumbnailUrl = thumbnailUrl;
        
        //AddDomainEvent(new PropertyCreatedDomainEvent(this.Id, propertyTypeId, hostId, name, description, floorNumber, rules, location, thumbnailUrl));
        if(hostId <= 0) throw new ArgumentException("hostId must be valid");
        if(propertyTypeId <= 0) throw new ArgumentException("propertyTypeId must be valid");
    }
    
    public Property(int id, int propertyTypeId, int hostId, string? name, string? description, int? floorNumber, HouseRule rules, Location location, string? thumbnailUrl):base(id)
    {
        PropertyTypeId = propertyTypeId;
        HostId = hostId;
        Name = name;
        Description = description;
        FloorNumber = floorNumber;
        Rules = rules;
        Location = location;
        ThumbnailUrl = thumbnailUrl;
    }
    
    // Domain Business Logics
    public void AddRentalUnit(RentalUnit rentalUnit)
    {
        ArgumentNullException.ThrowIfNull(rentalUnit);
        
        _rentalUnits.Add(rentalUnit);
        //AddDomainEvent(new RentalUnitAddedToPropertyDomainEvent(this.Id, rentalUnit.Id));
    }
}