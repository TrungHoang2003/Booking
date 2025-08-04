using BuildingBlocks.Interfaces;

namespace Contracts.Events;

public class RentalUnitAdded: IEvent
{
   public Guid CorrelationId { get; set; }
   public int RentalUnitId{ get; set; }
}