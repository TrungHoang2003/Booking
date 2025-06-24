namespace BuildingBlocks.DomainDrivenPattern;

public interface IDomainEvent
{
   DateTime OccurredOn { get; } 
}