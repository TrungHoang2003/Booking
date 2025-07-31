namespace Contracts.Messages;

public record UpdateHostProfile(
    Guid CorrelationId,
    int HostId,
    string? HostDescription,
    string? HostName 
    );