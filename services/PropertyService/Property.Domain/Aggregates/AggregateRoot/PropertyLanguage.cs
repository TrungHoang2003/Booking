using BuildingBlocks.DomainDrivenPattern;

namespace Property.Domain.Aggregates.AggregateRoot;

public class PropertyLanguage: Entity
{
    public int PropertyId { get; private set; }
    public int LanguageId { get; private set; }
    
    // Constructors
    public PropertyLanguage(int propertyId, int languageId)
    {
        PropertyId = propertyId;
        LanguageId = languageId;
    }
}