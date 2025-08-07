using BuildingBlocks.Interfaces;
using Contracts.Drafts;

namespace Contracts.Messages;

public class StartBecomeHost : IEvent
{
    public Guid CorrelationId { get; set; }
    public int HostId { get; set; }
    public BecomeHostDraft Draft { get; set; }
}
