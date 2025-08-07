using Property.Domain.Aggregates.AmenityAggregate;
using Property.Domain.Aggregates.ImageAggregate;
using Property.Domain.Aggregates.LanguageAggregate;
using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Domain.ValueObjects;

namespace Property.Domain.Aggregates.AggregateRoot;

public class Property : BuildingBlocks.Interfaces.AggregateRoot
{
    public int PropertyTypeId { get; private set; }
    public int HostId { get; private set; }
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public int? FloorNumber { get; private set; }
    public string? NeighborhoodDescription { get; private set; }
    public string? ThumbnailUrl { get; private set; }

    // Data Objects
    public HouseRule? Rules { get; private set; }
    public Location? Location { get; private set; }

    // Navigation Properties
    public PropertyType? Type { get; init; }
    public List<PropertyAmenity> Amenities = [];
    public List<PropertyLanguage> Languages = [];
    public List<RentalUnit> RentalUnits = [];

    public Property(int propertyTypeId, int hostId, string name, string? description, int? floorNumber,
        string? thumbnailUrl, string? neighborhoodDescription)
    {
        PropertyTypeId = propertyTypeId;
        HostId = hostId;
        Name = name;
        Description = description;
        FloorNumber = floorNumber;
        ThumbnailUrl = thumbnailUrl;
        NeighborhoodDescription = neighborhoodDescription;

        if (hostId <= 0) throw new ArgumentException("hostId must be valid");
        if (propertyTypeId <= 0) throw new ArgumentException("propertyTypeId must be valid");
    }

    // Domain Business Logics
    public void AddRentalUnit(RentalUnit rentalUnit)
    {
        ArgumentNullException.ThrowIfNull(rentalUnit);

        RentalUnits.Add(rentalUnit);
    }
    
    public void AddAmenity(Amenity amenity)
    {
        ArgumentNullException.ThrowIfNull(amenity);

        Amenities.Add(new PropertyAmenity(Id, amenity.Id, null));
        //AddDomainEvent(new PropertyAmenityAddedDomainEvent(this.Id, propertyAmenity.AmenityId));
    }
    
    public void AddDescription(string description)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description cannot be null or empty.", nameof(description));

        Description = description;
    }

    public void AddLanguage(Language language)
    {
        ArgumentNullException.ThrowIfNull(language);
        Languages.Add(new PropertyLanguage(Id, language.Id));
    }
    
    public void AddListLanguage(List<Language> languages)
    {
        ArgumentNullException.ThrowIfNull(languages);
        foreach (var language in languages)
        {
            Languages.Add(new PropertyLanguage(Id, language.Id));
        }
    }

    public void UpdateHouseRule(HouseRule houseRule)
    {
        ArgumentNullException.ThrowIfNull(houseRule);
        Rules = houseRule;
    }
    
    public void UpdateLocation(Location location)
    {
        ArgumentNullException.ThrowIfNull(location);
        Location = location;
    }
}