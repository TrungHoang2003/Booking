using BuildingBlocks.DomainDrivenPattern;

namespace Property.Domain.Aggregates.LanguageAggregate;

public class Language: Entity
{
    public string Name { get; private set; }
    
    // Constructors
    public Language(string name)
    {
        Name = name;
    }
    
    public Language(int id,string name)
    {
        Id = id;
        Name = name;
    }
}