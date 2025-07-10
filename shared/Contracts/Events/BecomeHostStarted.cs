namespace Shared.Contracts.Events;

public class BecomeHostStarted
{
    Guid CorrelataionId { get; set; }
    public int UserId { get; set; }
    public int PropertyTypeId { get; set; }
    public DateTime StartedAt { get; set; }
}