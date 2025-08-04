using BuildingBlocks.Interfaces;

namespace Contracts.Events;

public class BedroomAdded: IEvent
{
    public Guid CorrelationId { get; set; }
}