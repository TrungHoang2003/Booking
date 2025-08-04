
using BuildingBlocks.Interfaces;
using Contracts.DTOs;

namespace Contracts.Events;

public class BecomeHostStarted : IEvent
{
    public Guid CorrelationId { get; set; }
    public int HostId { get; set; }
}
