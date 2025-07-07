namespace Contracts.Events;

public record UpdateUserProfileEvent(
    Guid CorrelationId,
    int UserId,
    string FullName,
    string Description);