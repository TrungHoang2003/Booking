namespace Contracts.Commands;

public record UpdateUserProfile
{
    public Guid CorrelationId { get; init; } 
    public int UserId { get; init; }
    public string FullName { get; init; }
    public string Description { get; init; }
}