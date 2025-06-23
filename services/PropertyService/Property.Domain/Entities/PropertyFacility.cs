namespace Property.Domain.Entities;

public class PropertyFacility
{
    public Guid Id { get; set; }
    public Guid PropertyId { get; set; }
    public Guid FacilityId { get; set; }
}