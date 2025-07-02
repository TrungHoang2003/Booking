namespace Contracts.Events;

public record UpdateUserProfileEvent(
    int UserId,
    string FullName,
    string Description);