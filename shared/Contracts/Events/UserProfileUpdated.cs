namespace Contracts.Events;

public record UserProfileUpdated
{
   public Guid CorrelationId { get; init; } 
   public int UserId { get; init; }
   public string FullName { get; init; }
   public string Description { get; init; }
}
