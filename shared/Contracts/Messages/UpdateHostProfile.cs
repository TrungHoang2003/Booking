using BuildingBlocks.Interfaces;

namespace Contracts.Messages;

public class UpdateHostProfile: IMessage
{
    public Guid CorrelationId { get; set; }
    public int HostId { get; set; }
    public string? HostDescription { get; set; }
    public string? HostName { get; set; }
}