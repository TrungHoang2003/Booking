using BuildingBlocks.Interfaces;

namespace Contracts.Events;

public class PropertyCreated: IEvent
{
    public Guid CorrelationId { get; set; }
    public int PropertyId { get; set; }
}